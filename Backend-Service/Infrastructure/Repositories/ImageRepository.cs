using Backend_Service.Domain.Entities;
using Backend_Service.Infrastructure.Contexts;
using Backend_Service.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Service.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly ImageContext _context;

    public ImageRepository(ImageContext context)
    {
        _context = context;
    }

    public Task<Image> GetImage(Guid imageId)
    {
        var image = _context.Images
            .Select(i => i)
            .Where(i => i.Id == imageId)
            .FirstOrDefaultAsync();
        return image!;
    }

    public Task<Image[]> GetImages(int page, int pageSize)
    {
        var images = _context.Images
            .OrderBy(i => i.FileName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();
        
        return images;
        
    }

    public async Task<Image> UploadImage(Image image)
    {
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();
        
        var createdImage = await _context.Images
            .Where(i => i.FileName == image.FileName)
            .FirstOrDefaultAsync();
        
        return createdImage!;
    }
}