using Backend_Service.Domain.Entities;
using Backend_Service.Infrastructure.Contexts;
using Backend_Service.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Service.Infrastructure.Repositories;

public class SegmentationRepository : ISegmentationRepository
{
    private readonly ImageContext _context;
    
    public SegmentationRepository(ImageContext context)
    {
        _context = context;
    }
    
    public async Task<Segmentation> GetSegmentationById(Guid segmentationId)
    {
        var segmentation = await _context.Segmentations
            .Where(s => s.Id == segmentationId)
            .FirstOrDefaultAsync();

        return segmentation!;
    }
    
    public async Task<Segmentation[]> GetSegmentationsByImageAndLabel(Guid imageId, Guid labelId)
    {
        var segmentations = await _context.Segmentations
            .Where(s => s.ImageId == imageId && s.LabelId == labelId)
            .ToArrayAsync();

        return segmentations;
    }

    public async Task<Segmentation[]> GetSegmentationsByLabel(Guid labelId)
    {
        var segmentations = await _context.Segmentations
            .Where(s => s.LabelId == labelId)
            .ToArrayAsync();

        return segmentations;
    }

    public async Task<Segmentation[]> GetSegmentationsByImage(Guid imageId)
    {
        var segmentations = await _context.Segmentations
            .Where(s => s.ImageId == imageId)
            .ToArrayAsync();
        
        return segmentations;
    }

    public async Task<Segmentation> CreateSegmentation(Segmentation segmentation)
    {
        var createdSegmentation = await _context.Segmentations.AddAsync(segmentation);
        await _context.SaveChangesAsync();
        
        return createdSegmentation.Entity;
    }

    public async Task<Segmentation> UpdateSegmentation(Segmentation segmentation)
    {
        var updatedSegmentation = _context.Segmentations.Update(segmentation);
        await _context.SaveChangesAsync();
        return updatedSegmentation.Entity;
    }

    public async Task DeleteSegmentation(Segmentation segmentation)
    {
        _context.Segmentations.Remove(segmentation);
        await _context.SaveChangesAsync();
    }
}