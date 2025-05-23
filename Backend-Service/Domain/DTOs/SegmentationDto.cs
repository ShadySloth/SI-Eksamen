namespace Backend_Service.Domain.DTOs;

public class SegmentationDto
{
    /// The id of the segmentation.
    public Guid Id { get; set; }
    
    /// The first x-coordinate of the segmentation.
    public required double FirstCoordinateX { get; set; }
    
    /// The first y-coordinate of the segmentation.
    public required double FirstCoordinateY { get; set; }
    
    /// The second x-coordinate of the segmentation.
    public required double SecondCoordinateX { get; set; }
    
    /// The second y-coordinate of the segmentation.
    public required double SecondCoordinateY { get; set; }
    
    
    // Relations
    public Guid LabelId { get; set; }
    
    public LabelDto? Label { get; set; }
    
    public Guid ImageId { get; set; }
    
    public ImageDto? Image { get; set; }
}