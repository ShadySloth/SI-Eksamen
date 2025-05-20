using Backend_Service.Domain.DTOs;

namespace Backend_Service.Application.Interfaces;

public interface ISegmentationService
{
    public Task<SegmentationDto> GetSegmentationById(Guid segmentationId);
    
    public Task<SegmentationDto[]> GetSegmentationsByLabel(Guid labelId);
    
    public Task<SegmentationDto[]> GetSegmentationsByImage(Guid imageId);
    
    public Task<SegmentationDto[]> GetSegmentationsByImageAndLabel(Guid imageId, Guid labelId);
    
    public Task<SegmentationDto> CreateSegmentation(SegmentationDto segmentation);
    
    public Task<SegmentationDto> UpdateSegmentation(SegmentationDto segmentation);
    
    public Task DeleteSegmentation(Guid segmentationId);
}