using Backend_Service.Application.Interfaces;
using Backend_Service.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Service.Presentation;

[Route("[controller]")]
[ApiController]
public class LabelController : ControllerBase
{
    private readonly ILabelService _labelService;
    
    public LabelController(ILabelService labelService)
    {
        _labelService = labelService;
    }

    /// <summary>
    /// Gets all labels.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<LabelDto[]>> GetLabels()
    {
        var labels = await _labelService.GetLabels();

        return Ok(labels);
    }

    /// <summary>
    /// Gets a label by id.
    /// </summary>
    /// <param name="labelId"></param>
    /// <returns></returns>
    public async Task<ActionResult<LabelDto>> GetLabell(Guid labelId)
    {
        var label = await _labelService.GetLabel(labelId);

        return Ok(label);
    }
    
    /// <summary>
    /// Creates a new label.
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    public async Task<ActionResult<LabelDto>> CreateLabel([FromBody] LabelDto label)
    {
        var createdLabel = await _labelService.CreateLabel(label);

        return CreatedAtAction(nameof(GetLabell), new { labelId = createdLabel.Id }, createdLabel);
    }
    
    /// <summary>
    /// Updates an existing label.
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    public async Task<ActionResult<LabelDto>> UpdateLabel([FromBody] LabelDto label)
    {
        var updatedLabel = await _labelService.UpdateLabel(label);

        return Ok(updatedLabel);
    }
    
    /// <summary>
    /// Deletes a label by id.
    /// </summary>
    /// <param name="labelId"></param>
    /// <returns></returns>
    public async Task<ActionResult> DeleteLabel(Guid labelId)
    {
        await _labelService.DeleteLabel(labelId);

        return NoContent();
    }
}