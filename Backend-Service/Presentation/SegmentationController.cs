using Backend_Service.Application.Interfaces;
using Backend_Service.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Service.Presentation;

[Route("[controller]")]
[ApiController]
public class SegmentationController : ControllerBase
{
    private readonly ISegmentationService _segmentationService;
    
    public SegmentationController(ISegmentationService segmentationService)
    {
        _segmentationService = segmentationService;
    }
    
    [HttpGet]
    public async Task<ActionResult<SegmentationDto>> GetSegmentationById([FromQuery]Guid segmentationId)
    {
        var segmentationDto = await _segmentationService.GetSegmentationById(segmentationId);
        return Ok(segmentationDto);
    }
    
    [HttpGet]
    public async Task<ActionResult<SegmentationDto[]>> GetSegmentationsByLabel([FromQuery]Guid labelId)
    {
        var segmentationDtos = await _segmentationService.GetSegmentationsByLabel(labelId);
        return Ok(segmentationDtos);
    }

    [HttpGet]
    public async Task<ActionResult<SegmentationDto[]>> GetSegmentationsByImage([FromQuery]Guid imageId)
    {
        var segmentationDtos = await _segmentationService.GetSegmentationsByImage(imageId);
        return Ok(segmentationDtos);
    }
    
    [HttpPost]
    public async Task<ActionResult<SegmentationDto>> CreateSegmentation([FromBody] SegmentationDto segmentation)
    {
        var segmentationDto = await _segmentationService.CreateSegmentation(segmentation);
        return CreatedAtAction(nameof(GetSegmentationById), new { segmentationId = segmentationDto.Id }, segmentationDto);
    }
    
    [HttpPut]
    public async Task<ActionResult<SegmentationDto>> UpdateSegmentation([FromBody] SegmentationDto segmentation)
    {
        var segmentationDto = await _segmentationService.UpdateSegmentation(segmentation);
        return Ok(segmentationDto);
    }
    
    [HttpDelete("{segmentationId}")]
    public async Task<IActionResult> DeleteSegmentation(Guid segmentationId)
    {
        await _segmentationService.DeleteSegmentation(segmentationId);
        return NoContent();
    }
}