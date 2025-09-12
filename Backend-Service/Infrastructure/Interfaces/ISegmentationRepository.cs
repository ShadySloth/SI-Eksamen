using Backend_Service.Domain.Entities;

namespace Backend_Service.Infrastructure.Interfaces;

public interface ISegmentationRepository
{
    Task<Segmentation> GetSegmentationById(int segmentationId);
    Task<Segmentation[]> GetSegmentationsByImageAndLabel(int imageId, int labelId);
    Task<Segmentation[]> GetSegmentationsByLabel(int labelId);
    Task<Segmentation[]> GetSegmentationsByImage(int imageId);
    Task<Segmentation> CreateSegmentation(Segmentation segmentation);
    Task<Segmentation> UpdateSegmentation(Segmentation segmentation);
    Task DeleteSegmentation(Segmentation segmentationId);
}