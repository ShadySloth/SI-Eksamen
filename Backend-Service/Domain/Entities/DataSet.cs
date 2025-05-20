namespace Backend_Service.Domain.Entities;

public class DataSet
{
    /// The id of the dataset.
    public Guid Id { get; set; }
    
    /// The name of the dataset.
    public required string DataSetName { get; set; }
}