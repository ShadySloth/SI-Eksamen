namespace Backend_Service.Domain.Entities;

public class Segmentation
{
    /// The id of the segmentation.
    public Guid Id { get; set; }
    
    /// The first coordinate of the segmentation.
    public double FirstCoordinate { get; set; }
    
    /// The second coordinate of the segmentation.
    public double SecondCoordinate { get; set; }
    
    // Relations
    public Guid LabelId { get; set; }
    
    public Label Label { get; set; }
    
    public Guid ImageId { get; set; }
    
    public Image Image { get; set; }
}