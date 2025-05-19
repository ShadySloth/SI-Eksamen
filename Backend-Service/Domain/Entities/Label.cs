namespace Backend_Service.Domain.Entities;

public class Label
{
    /// The id of the label
    public Guid Id { get; set; }
    
    /// The name of the label
    public required string Name { get; set; }
    
    /// Array of image ids with label
    public List<Guid> ImageIds { get; set; } = [];
    
    /// Array of images with label
    public List<Image> Images { get; set; } = [];
}