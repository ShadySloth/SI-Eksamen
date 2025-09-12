using Backend_Service.Domain.DTOs;

namespace Backend_Service.Application.Interfaces;

public interface ISegmentationService
{
    public Task<SegmentationDto> GetSegmentationById(int segmentationId);
    public Task<SegmentationDto[]> GetSegmentationsByImageAndLabel(int imageId, int labelId);
    public Task<SegmentationDto[]> GetSegmentationsByLabel(int labelId);
    
    public Task<SegmentationDto[]> GetSegmentationsByImage(int imageId);
    
    public Task<SegmentationDto> CreateSegmentation(SegmentationDto segmentation);
    
    public Task<SegmentationDto> UpdateSegmentation(SegmentationDto segmentation);
    
    public Task DeleteSegmentation(int segmentationId);
}