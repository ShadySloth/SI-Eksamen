using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Service.Application.Interfaces;

public interface IImageService
{
    /// <summary>
    /// Gets an image by id.
    /// </summary>
    /// <param name="imageId">Id to get by.</param>
    /// <returns>ImageDto</returns>
    Task<ImageDto> GetImage(Guid imageId);

    /// <summary>
    /// Gets paginated images.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns>Array of ImageDto</returns>
    Task<ImageDto[]> GetImages(int page, int pageSize);
    
    /// <summary>
    /// Uploads an image.
    /// </summary>
    /// <param name="file">File to upload</param>
    /// <returns>ImageDto</returns>
    Task<ImageDto> UploadImage(IFormFile file);
}