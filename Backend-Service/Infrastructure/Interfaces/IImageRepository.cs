using Backend_Service.Domain.Entities;

namespace Backend_Service.Infrastructure.Interfaces;

public interface IImageRepository
{
    /**
     * Gets an image by id.
     */
    Task<Image> GetImage(Guid imageId);

    /**
     * Gets a paginated list of images.
     */
    Task<Image[]> GetImages(int page, int pageSize);
    
    /**
     * Uploads an image.
     */
    Task<Image> UploadImage(Image image);
}