namespace Backend_Service.Domain.DTOs;

public class DataSetDto
{
    /// The id of the dataset.
    public Guid Id { get; set; }
    
    /// The name of the dataset.
    public required string DataSetName { get; set; }
}