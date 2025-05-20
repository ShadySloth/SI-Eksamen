namespace Backend_Service.Domain.DTOs;

public class ImageDto
{
    /// The id of the image.
    public Guid Id { get; set; }
    
    /// The file name of the image.
    public required string FileName { get; set; }
    
    /// The base64 string of the image file.
    public string? FileBase64 { get; set; }
    
    //Relations
    public List<LabelDto> Labels { get; set; } = [];
}