using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Service.Application.Interfaces;

public interface IImageService
{
    /**
     * Gets an image by id.
     */
    Task<ImageDto> GetImage(Guid imageId);

    /**
     * Gets paginated images.
     */
    Task<ImageDto[]> GetImages(int page, int pageSize);
    
    /**
     * Uploads an image.
     */
    Task<ImageDto> UploadImage(IFormFile file);
}