using System.IO.Compression;
using System.Security.Cryptography;
using AutoMapper;
using Backend_Service.Application.Interfaces;
using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;
using Backend_Service.Infrastructure.Interfaces;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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
        var images = new List<ImageDto>();
        foreach (var label in dataSetDto.LabelsToBeUsed)
        {
            images.AddRange(_imageService.GetImagesByLabel(label).Result); 
        }

        foreach (var (label, labelIndex) in dataSetDto.LabelsToBeUsed.Select((l, i) => (l, i)))
        {
            foreach (var image in images)
            {
                var segments = await _segmentationService.GetSegmentationsByImageAndLabel(image.Id, label);
                collectedSegments.AddRange(segments.Select(segment => (segment, labelIndex)));
            }
        }
        
        // Group segments by image
        var random = new Random();
        var grouped = collectedSegments
            .GroupBy(x => x.segmentationDto.ImageId)
            .OrderBy(_ => random.Next())
            .ToList();

        var total = grouped.Count;
        var trainCount = (int)(total * 0.9);
        var validCount = (int)(total * 0.08);
        var testCount = total - trainCount - validCount;

        var trainGroup = grouped.Take(trainCount);
        var validGroup = grouped.Skip(trainCount).Take(validCount);
        var testGroup = grouped.Skip(trainCount + validCount).Take(testCount);

        await WriteSet(trainGroup, "train", dataSetDto.DataSet.DataSetName);
        await WriteSet(validGroup, "valid", dataSetDto.DataSet.DataSetName);
        await WriteSet(testGroup, "test", dataSetDto.DataSet.DataSetName);

        // Save dataset to database
        var dataSetToCreate = _mapper.Map<DataSet>(dataSetDto.DataSet);
        var createdDataSet = await _dataRepository.CreateDataSet(dataSetToCreate);
        var dataSetResult = _mapper.Map<DataSetDto>(createdDataSet);

        // Generate yaml file
        var config = new DataSetConfig
        {
            Train = "../train/images",
            Val = "../valid/images",
            Test = "../test/images",
            Nc = dataSetDto.LabelsToBeUsed.Length,
            Names = dataSetDto.LabelsToBeUsed.Select(l => l.Name).ToArray()
        };

        var yamlPath = $"../blob/datasets/{dataSetResult.DataSetName}/data.yaml";
        await WriteYamlConfig(config, yamlPath);
        
        // Zip the dataset
        ZipFile.CreateFromDirectory($"../blob/datasets/{dataSetResult.DataSetName}",
            $"../blob/datasets/{dataSetResult.DataSetName}.zip");

        return dataSetResult;
    }

    public async Task<DataSetDto[]> GetDataSets()
    {
        var dataSets = await _dataRepository.GetDataSets();
        var dataSetDtos = _mapper.Map<DataSetDto[]>(dataSets);
        return dataSetDtos;
    }

    private async Task WriteYamlConfig(DataSetConfig config, string filePath)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var yaml = serializer.Serialize(config);
        await File.WriteAllTextAsync(filePath, yaml);
    }
    
    private async Task WriteSet(IEnumerable<IGrouping<Guid, (SegmentationDto segmentationDto, int labelIndex)>> groups,
        string setName, string dataSetName)
    {
        foreach (var group in groups)
        {
            foreach (var (segment, labelIndex) in group)
            {
                var fileName = $"{segment.ImageId}_{GenerateRandomString(32)}".Replace('.', '_');
                var labelDir = $"../blob/datasets/{dataSetName}/{setName}/labels";
                var imageDir = $"../blob/datasets/{dataSetName}/{setName}/images";
                if (!Directory.Exists(labelDir))
                    Directory.CreateDirectory(labelDir);
                if (!Directory.Exists(imageDir))
                    Directory.CreateDirectory(imageDir);

                var labelPath = Path.Combine(labelDir, $"{fileName}.txt");
                var content = $"{labelIndex} " +
                              $"{segment.FirstCoordinateX} {segment.FirstCoordinateY} " +
                              $"{segment.SecondCoordinateX} {segment.SecondCoordinateY}";
                await File.WriteAllTextAsync(labelPath, content);
                
                var image = await _imageService.GetImage(segment.ImageId);
                var extension = Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(imageDir, $"{fileName}{extension}");
                var imageBytes = Convert.FromBase64String(image.FileBase64!);
                await File.WriteAllBytesAsync(imagePath, imageBytes);
            }
        }
    }
    
    private static async Task WriteSegmentToFile(SegmentationDto segment, string fileName, string dataSetName,
        int labelIndex)
    {
        var train = $"../blob/datasets/{dataSetName}/train/labels";
        var test = $"../blob/datasets/{dataSetName}/test/labels";
        var valid = $"../blob/datasets/{dataSetName}/valid/labels";

        if (!Directory.Exists(train))
            Directory.CreateDirectory(train);
        var fullFilePath = Path.Combine(train, $"{fileName}.txt");

        var content = $"{labelIndex} " +
                      $"{segment.FirstCoordinateX} {segment.FirstCoordinateY} " +
                      $"{segment.SecondCoordinateX} {segment.SecondCoordinateY}";

        await File.WriteAllTextAsync(fullFilePath, content);
    }

    private static async Task WriteImageToFile(string imageBase64, string fileName, string dataSetName,
        string originalFileName)
    {
        var directoryPath = $"../blob/datasets/{dataSetName}/train/images";

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