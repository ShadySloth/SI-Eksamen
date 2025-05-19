using Backend_Service.Domain.Entities;

namespace Backend_Service.Infrastructure.Interfaces;

public interface IImageRepository
{
    /**
     * Gets an image by id.
     */
    Task<Image> GetImage(Guid imageId);
    
    /**
     * Uploads an image.
     */
    Task<Image> UploadImage(Image image);
}