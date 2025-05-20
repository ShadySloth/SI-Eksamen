using Backend_Service.Domain.Entities;

namespace Backend_Service.Domain.DTOs;

public class CreateDataSetDto
{
    /// The DataSet to be created.
    public required DataSetDto DataSet { get; set; }
    
    /// The images to be used in the dataset.
    public ImageDto[] Images { get; set; } = [];
    
    /// The labels to be used in the dataset.
    public LabelDto[] LabelsToBeUsed { get; set; } = [];
}