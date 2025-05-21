using Backend_Service.Domain.DTOs;
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

    public async Task<PagedResult<Image>> GetImages(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        
        var totalCount = await _context.Images.CountAsync();
        var images = await _context.Images
            .OrderBy(i => i.FileName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();
        
        var pagedResult = new PagedResult<Image>
        {
            Items = images,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
        
        return pagedResult;
    }

    public async Task<Image[]> GetImagesByLabel(Guid labelId)
    {
        var images = await _context.Images
            .Where(i => i.Labels.Any(l => l.Id ==labelId))
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