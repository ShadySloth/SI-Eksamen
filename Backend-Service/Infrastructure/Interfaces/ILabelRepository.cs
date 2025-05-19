using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;

namespace Backend_Service.Infrastructure.Interfaces;

public interface ILabelRepository
{
    /// <summary>
    /// Gets all labels.
    /// </summary>
    /// <returns></returns>
    public Task<Label[]> GetLabels();
    
    /// <summary>
    /// Gets a label by id.
    /// </summary>
    /// <param name="labelId"></param>
    /// <returns></returns>
    public Task<Label> GetLabel(Guid labelId);
    
    /// <summary>
    /// Gets a label with images by id.
    /// </summary>
    /// <param name="labelId"></param>
    /// <returns></returns>
    public Task<Label> GetLabelWithImages(Guid labelId);
    
    /// <summary>
    /// Creates a new label.
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    public Task<Label> CreateLabel(Label label);
    
    /// <summary>
    /// Updates an existing label.
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    public Task<Label> UpdateLabel(Label label);
    
    /// <summary>
    /// Deletes a label by id.
    /// </summary>
    /// <param name="labelId"></param>
    /// <returns></returns>
    public Task DeleteLabel(Guid labelId);
}