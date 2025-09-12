namespace Backend_Service.Domain.DTOs;

public class LabelDto
{
    /// The id of the label.
    public int Id { get; set; }
    
    /// The name of the label.
    public required string Name { get; set; }
    
    
    // Relations
    public List<ImageDto> Images { get; set; } = [];
}