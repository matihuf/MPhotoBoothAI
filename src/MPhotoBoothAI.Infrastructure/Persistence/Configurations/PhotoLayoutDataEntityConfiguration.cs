using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Infrastructure.Persistence.Configurations;
public class PhotoLayoutDataEntityConfiguration : IEntityTypeConfiguration<PhotoLayoutDataEntity>
{
    public void Configure(EntityTypeBuilder<PhotoLayoutDataEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Top).HasColumnType("REAL").IsRequired();
        builder.Property(x => x.Left).HasColumnType("REAL").IsRequired();
        builder.Property(x => x.Angle).HasColumnType("REAL").IsRequired();
        builder.Property(x => x.Scale).HasColumnType("REAL").IsRequired();
    }
}
