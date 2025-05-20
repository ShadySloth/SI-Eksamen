using Backend_Service.Application.Interfaces;
using Backend_Service.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Service.Presentation;

[Route("[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    
    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    /// <summary>
    /// Gets an image by id.
    /// </summary>
    /// <param name="imageId"></param>
    /// <returns></returns>
    [HttpGet("{imageId}")]
    public async Task<ActionResult<ImageDto>> GetImageAsync(Guid imageId)
    {
        var image = await _imageService.GetImage(imageId);
        return image;
    }

    /// <summary>
    /// Gets paginated images.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<ImageDto[]>> GetImages(int page = 1, int pageSize = 10)
    {
        var images = await _imageService.GetImages(page, pageSize);
        return Ok(images);
    }
    
    /// <summary>
    /// Uploads an image.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UploadImageAsync(IFormFile file)
    {
        var image = await _imageService.UploadImage(file);
        return Ok(image);
    }
}