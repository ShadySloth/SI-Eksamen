using Backend_Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend_Service.Infrastructure.Contexts;

public class ImageContext : DbContext
{
    public ImageContext(DbContextOptions<ImageContext> options) : base(options)
    {
    }
    
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Image
        modelBuilder.Entity<Image>()
            .Property(i => i.FileName)
            .HasMaxLength(50);
        modelBuilder.Entity<Image>()
            .HasIndex(i => i.FileName)
            .IsUnique();
    }
}