namespace Backend_Service.Domain.DTOs;

public class SegmentationDto
{
    public Guid Id { get; set; }
    public double FirstCoordinate { get; set; }
    public double SecondCoordinate { get; set; }
    
    public Guid LabelId { get; set; }
    
    public LabelDto Label { get; set; }
    
    public Guid ImageId { get; set; }
    
    public ImageDto Image { get; set; }
}