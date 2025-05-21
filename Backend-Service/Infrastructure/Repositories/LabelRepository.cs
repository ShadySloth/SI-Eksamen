using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;
using Backend_Service.Infrastructure.Contexts;
using Backend_Service.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Service.Infrastructure.Repositories;

public class LabelRepository : ILabelRepository
{
    private readonly ImageContext _context;
    
    public LabelRepository(ImageContext context)
    {
        _context = context;
    }
    
    public async Task<Label[]> GetLabels()
    {
        var labels = await _context.Labels.Include(label => label.Images).ToArrayAsync();

        return labels;
    }

    public async Task<Label> GetLabel(Guid labelId)
    {
        var label = await _context.Labels
            .FirstOrDefaultAsync(l => l.Id == labelId);
        return label!;
    }

    public async Task<Label> GetLabelWithImages(Guid labelId)
    {
        var label = await _context.Labels
            .Where(l => l.Id == labelId)
            .Include(l => l.Images)
            .FirstOrDefaultAsync();
        
        return label!;
    }

    public async Task<Label> CreateLabel(Label label)
    {
        var createdLabel = await _context.Labels.AddAsync(label);
        await _context.SaveChangesAsync();

        return createdLabel.Entity;
    }

public async Task<Label> UpdateLabel(Label label)
{
    var existingLabel = await _context.Labels
        .Include(l => l.Images)
        .FirstOrDefaultAsync(l => l.Id == label.Id);

    if (existingLabel == null)
        throw new Exception("Label not found");

    // Update scalar properties
    existingLabel.Name = label.Name;

    // Sync Images collection
    // 1. Remove images no longer in the new list
    var incomingImageIds = label.Images.Select(i => i.Id).ToHashSet();
    existingLabel.Images.RemoveAll(img => !incomingImageIds.Contains(img.Id));

    // 2. Add or update images from the new list
    foreach (var image in label.Images)
    {
        var existingImage = existingLabel.Images.FirstOrDefault(i => i.Id == image.Id);
        if (existingImage != null)
        {
            continue;
        }
        else
        {
            // Attach the new image
            existingLabel.Images.Add(image);
        }
    }

    await _context.SaveChangesAsync();
    return existingLabel;
}

    public async Task DeleteLabel(Guid labelId)
    {
        var label = await _context.Labels
            .FirstOrDefaultAsync(l => l.Id == labelId);
        
        _context.Labels.Remove(label!);
        await _context.SaveChangesAsync();
    }
}