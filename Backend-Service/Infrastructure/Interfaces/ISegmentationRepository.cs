using Backend_Service.Domain.Entities;

namespace Backend_Service.Infrastructure.Interfaces;

public interface ISegmentationRepository
{
    Task<Segmentation> GetSegmentationById(Guid segmentationId);
    Task<Segmentation[]> GetSegmentationsByImageAndLabel(Guid imageId, Guid labelId);
    Task<Segmentation[]> GetSegmentationsByLabel(Guid labelId);
    Task<Segmentation[]> GetSegmentationsByImage(Guid imageId);
    Task<Segmentation> CreateSegmentation(Segmentation segmentation);
    Task<Segmentation> UpdateSegmentation(Segmentation segmentation);
    Task DeleteSegmentation(Guid segmentationId);
}