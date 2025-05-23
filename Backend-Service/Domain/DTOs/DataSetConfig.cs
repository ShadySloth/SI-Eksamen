namespace Backend_Service.Domain.DTOs;

public class DataSetConfig
{
    public string? Train { get; set; }
    public string? Val { get; set; }
    public string? Test { get; set; }
    public int? Nc { get; set; }
    public string[]? Names { get; set; }
}