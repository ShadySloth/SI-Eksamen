using AutoMapper;
using Backend_Service.Application.Interfaces;
using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;
using Backend_Service.Infrastructure.Interfaces;

namespace Backend_Service.Application.Services;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;

    public ImageService(IMapper mapper, IImageRepository imageRepository)
    {
        _mapper = mapper;
        _imageRepository = imageRepository;
    }

    public async Task<ImageDto> GetImage(Guid imageId)
    {
        var image = await _imageRepository.GetImage(imageId);
        var imageDto = _mapper.Map<ImageDto>(image);

        imageDto.FileBase64 = GetFile(imageDto.FileName);
        
        return imageDto;
    }

    public async Task<ImageDto> UploadImage(IFormFile file)
    {
        CheckFile(file);
        
        //upload file to blob or local storage
        StoreLocally(file);

        var image = new Image
        {
            FileName = file.FileName
        };
        
        var createdImage = await _imageRepository.UploadImage(image);

        var imageDto = _mapper.Map<ImageDto>(createdImage);
        imageDto.FileBase64 = GetFile(imageDto.FileName);
        return imageDto;
    }

    /// Store the file locally
    private static void StoreLocally(IFormFile file)
    {
        var root = Directory.GetCurrentDirectory();
        var path = Path.Combine(root, "Images");
        
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
        var filePath = Path.Combine(path, file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
            stream.Close();
        }
    }
    
    /// Check if the file is empty and has a valid extension.
    private static void CheckFile(IFormFile file)
    {
        if (file.Length == 0)
            throw new Exception("File is empty");
        
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName);
        if (!allowedExtensions.Contains(extension))
            throw new Exception("File type is not allowed");
    }

    private static string GetFile(string fileName)
    {
        var root = Directory.GetCurrentDirectory();
        var path = Path.Combine(root, "Images", fileName);
        
        if (!File.Exists(path))
            throw new Exception("File not found");

        using var stream = new FileStream(path, FileMode.Open);
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return Convert.ToBase64String(memoryStream.ToArray());
    }
}