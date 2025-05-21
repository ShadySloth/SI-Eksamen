using Backend_Service.Domain.DTOs;
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
    Task<PagedResult<Image>> GetImages(int page, int pageSize);
    
    /// Gets images by label id.
    Task<Image[]> GetImagesByLabel(Guid labelId);
    
    /**
     * Uploads an image.
     */
    Task<Image> UploadImage(Image image);
}