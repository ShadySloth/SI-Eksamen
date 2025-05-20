using Backend_Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend_Service.Infrastructure.Contexts;

public class ImageContext : DbContext
{
    public ImageContext(DbContextOptions<ImageContext> options) : base(options)
    {
    }
    
    public DbSet<Image> Images { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Segmentation> Segmentations { get; set; }
    public DbSet<DataSet> DataSets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Image
        modelBuilder.Entity<Image>()
            .Property(i => i.FileName)
            .HasMaxLength(50);
        modelBuilder.Entity<Image>()
            .HasIndex(i => i.FileName)
            .IsUnique();
        
        //Label
        modelBuilder.Entity<Label>()
            .Property(l => l.Name)
            .HasMaxLength(50);
        modelBuilder.Entity<Label>()
            .HasMany(l => l.Images)
            .WithMany(i => i.Labels);
        
        //Segmentation
        modelBuilder.Entity<Segmentation>()
            .HasOne(s => s.Label)
            .WithMany()
            .HasForeignKey(s => s.LabelId);
        modelBuilder.Entity<Segmentation>()
            .HasOne(s => s.Image)
            .WithMany()
            .HasForeignKey(s => s.ImageId);
    }
}