namespace Backend_Service.Domain.Entities;

public class Label
{
    /// The id of the label
    public int Id { get; set; }
    
    /// The name of the label
    public required string Name { get; set; }
    
    // Relations
    public List<Image> Images { get; set; } = [];
}