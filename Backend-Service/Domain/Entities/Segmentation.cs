namespace Backend_Service.Domain.Entities;

public class Segmentation
{
    /// The id of the segmentation.
    public Guid Id { get; set; }

    /// The first x-coordinate of the segmentation.
    public required double FirstCoordinateX { get; set; }

    /// The first y-coordinate of the segmentation.
    public required double FirstCoordinateY { get; set; }

    /// The second x-coordinate of the segmentation.
    public required double SecondCoordinateX { get; set; }

    /// The second y-coordinate of the segmentation.
    public required double SecondCoordinateY { get; set; }

    // Relations
    public Guid LabelId { get; set; }

    public required Label Label { get; set; }

    public Guid ImageId { get; set; }

    public required Image Image { get; set; }
}