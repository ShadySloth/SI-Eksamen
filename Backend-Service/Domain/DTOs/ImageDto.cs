namespace Backend_Service.Domain.DTOs;

public class ImageDto
{
    public Guid Id { get; set; }
    public required string FileName { get; set; }
    public required IFormFile File { get; set; }
}