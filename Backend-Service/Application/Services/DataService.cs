using System.Security.Cryptography;
using AutoMapper;
using Backend_Service.Application.Interfaces;
using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;
using Backend_Service.Infrastructure.Interfaces;

namespace Backend_Service.Application.Services;

public class DataService : IDataService
{
    private readonly IDataRepository _dataRepository;
    private readonly IImageService _imageService;

    private readonly ISegmentationService _segmentationService;

    private readonly IMapper _mapper;

    public DataService(IDataRepository dataRepository, IMapper mapper, IImageService imageService,
        ISegmentationService segmentationService)
    {
        _dataRepository = dataRepository;
        _segmentationService = segmentationService;
        _imageService = imageService;
        _mapper = mapper;
    }

    public async Task<DataSetDto> CreateTrainingSet(CreateDataSetDto dataSetDto)
    {
        // Create dataset files and save them to blob or local storage
        var collectedSegments = new List<(SegmentationDto segmentationDto, int labelIndex)>();

        foreach (var (label, labelIndex) in dataSetDto.LabelsToBeUsed.Select((l, i) => (l, i)))
        {
            foreach (var image in dataSetDto.Images)
            {
                var segments = await _segmentationService.GetSegmentationsByImageAndLabel(image.Id, label.Id);
                collectedSegments.AddRange(segments.Select(segment => (segment, labelIndex)));
            }
        }

        foreach (var (segment, labelIndex) in collectedSegments)
        {
            // Save segment to file
            var fileName = $"{segment.ImageId}_{GenerateRandomString(32)}".Replace('.', '_');
            await WriteSegmentToFile(segment, fileName, dataSetDto.DataSet.DataSetName, labelIndex);
            // Save image to file
            var image = await _imageService.GetImage(segment.ImageId);
            await WriteImageToFile(image.FileBase64!, fileName, dataSetDto.DataSet.DataSetName, image.FileName);
        }


        // Save dataset to database
        var dataSetToCreate = _mapper.Map<DataSet>(dataSetDto.DataSet);
        var createdDataSet = await _dataRepository.CreateDataSet(dataSetToCreate);
        var dataSetResult = _mapper.Map<DataSetDto>(createdDataSet);
        return dataSetResult;
    }

    private static async Task WriteSegmentToFile(SegmentationDto segment, string fileName, string dataSetName,
        int labelIndex)
    {
        var directoryPath = $"../{dataSetName}/train/labels";

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        var fullFilePath = Path.Combine(directoryPath, $"{fileName}.txt");

        var content = $"{labelIndex} " +
                      $"{segment.FirstCoordinateX} {segment.FirstCoordinateY} " +
                      $"{segment.SecondCoordinateX} {segment.SecondCoordinateY}";

        await File.WriteAllTextAsync(fullFilePath, content);
    }

    private static async Task WriteImageToFile(string imageBase64, string fileName, string dataSetName,
        string originalFileName)
    {
        var directoryPath = $"../{dataSetName}/train/images";

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        var extension = Path.GetExtension(originalFileName);
        var fullFilePath = Path.Combine(directoryPath, $"{fileName}{extension}");

        var imageBytes = Convert.FromBase64String(imageBase64);
        await File.WriteAllBytesAsync(fullFilePath, imageBytes);
    }

    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            var data = new byte[length];
            rng.GetBytes(data);
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[data[i] % chars.Length];
            }
        }

        return new string(stringChars);
    }
}