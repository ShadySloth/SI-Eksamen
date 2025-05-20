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
        var updatedLabel = _context.Labels.Update(label);
        await _context.SaveChangesAsync();

        return updatedLabel.Entity;
    }

    public async Task DeleteLabel(Guid labelId)
    {
        var label = await _context.Labels
            .FirstOrDefaultAsync(l => l.Id == labelId);
        
        _context.Labels.Remove(label!);
        await _context.SaveChangesAsync();
    }
}