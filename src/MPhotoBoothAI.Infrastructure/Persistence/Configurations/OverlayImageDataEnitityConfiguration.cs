using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Infrastructure.Persistence.Configurations;
public class OverlayImageDataEnitityConfiguration : IEntityTypeConfiguration<OverlayImageDataEntity>
{
    public void Configure(EntityTypeBuilder<OverlayImageDataEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Left).HasColumnType("REAL").IsRequired();
        builder.Property(x => x.Top).HasColumnType("REAL").IsRequired();
        builder.Property(x => x.Angle).HasColumnType("REAL").IsRequired();
        builder.Property(x => x.Scale).HasColumnType("REAL").IsRequired();
        builder.Property(x => x.Path)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100)
            .IsRequired();
    }
}
