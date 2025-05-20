namespace Backend_Service.Domain.Entities;

public class Image
{
    /// The id of the image
    public Guid Id { get; set; }
    
    /// The file name of the image
    public required string FileName { get; set; }
}