using Backend_Service.Domain.DTOs;

namespace Backend_Service.Application.Interfaces;

public interface ILabelService
{
    /// <summary>
    /// Gets all labels.
    /// </summary>
    /// <returns>Array of LabelDto</returns>
    public Task<LabelDto[]> GetLabels();
    
    /// <summary>
    /// Gets a label by id.
    /// </summary>
    /// <param name="labelId">Id to get by.</param>
    /// <returns>LabelDto</returns>
    public Task<LabelDto> GetLabel(Guid labelId);
    
    /// <summary>
    /// Gets a label with images by id.
    /// </summary>
    /// <param name="labelId"></param>
    /// <returns></returns>
    public Task<LabelDto> GetLabelWithImages(Guid labelId);
    
    /// <summary>
    /// Creates a new label.
    /// </summary>
    /// <returns>LabelDto</returns>
    public Task<LabelDto> CreateLabel(LabelDto label);
    
    /// <summary>
    /// Updates an existing label.
    /// </summary>
    /// <param name="label"></param>
    /// <returns>LabelDto</returns>
    public Task<LabelDto> UpdateLabel(LabelDto label);
    
    /// <summary>
    /// Deletes a label by id.
    /// </summary>
    /// <param name="labelId">Id to delete by.</param>
    /// <returns></returns>
    public Task DeleteLabel(Guid labelId);
}