namespace Backend_Service.Domain.DTOs;

public class LabelDto
{
    /// The id of the label.
    public Guid Id { get; set; }
    
    /// The name of the label.
    public required string Name { get; set; }
    
    /// Array of image ids with label.
    public List<Guid> ImageIds { get; set; } = [];
    
    /// Array of images with label.
    public List<ImageDto> Images { get; set; } = [];
}