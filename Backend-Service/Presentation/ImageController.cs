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

    /**
     * Gets an image by id.
     */
    [HttpGet("{imageId}")]
    public async Task<ActionResult<ImageDto>> GetImageAsync(Guid imageId)
    {
        var image = await _imageService.GetImage(imageId);
        return image;
    }

    [HttpGet]
    public async Task<ActionResult<ImageDto[]>> GetImages(int page = 1, int pageSize = 20)
    {
        var images = await _imageService.GetImages(page, pageSize);
        return Ok(images);
    }
    
    /**
     * Uploads an image.
     */
    [HttpPost]
    public async Task<IActionResult> UploadImageAsync(IFormFile file)
    {
        var image = await _imageService.UploadImage(file);
        return Ok(image);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok();
    }
}