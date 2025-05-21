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
    
    [HttpGet("{segmentationId}")]
    public async Task<ActionResult<SegmentationDto>> GetSegmentationById(Guid segmentationId)
    {
        var segmentationDto = await _segmentationService.GetSegmentationById(segmentationId);
        return Ok(segmentationDto);
    }

    [HttpGet("imageandlabel")]
    public async Task<ActionResult<SegmentationDto[]>> GetSegmentationByImageAndLabel([FromQuery]Guid imageId, [FromQuery]Guid labelId)
    {
        var segmentationDtos = await _segmentationService.GetSegmentationsByImageAndLabel(imageId, labelId);
        return Ok(segmentationDtos);
    }
    
    [HttpGet("byLabel/{labelId}")]
    public async Task<ActionResult<SegmentationDto[]>> GetSegmentationsByLabel(Guid labelId)
    {
        var segmentationDtos = await _segmentationService.GetSegmentationsByLabel(labelId);
        return Ok(segmentationDtos);
    }

    [HttpGet("byImage/{imageId}")]
    public async Task<ActionResult<SegmentationDto[]>> GetSegmentationsByImage(Guid imageId)
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