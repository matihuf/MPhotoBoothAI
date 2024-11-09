using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Infrastructure.Persistence.Configurations
{
    public class CameraSettingsEntityConfiguration : IEntityTypeConfiguration<CameraSettingsEntity>
    {
        public void Configure(EntityTypeBuilder<CameraSettingsEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Camera)
            .HasMaxLength(50)
            .HasColumnType("NVARCHAR")
            .IsRequired();
            builder.Property(x => x.Iso)
            .HasMaxLength(20)
            .HasColumnType("NVARCHAR")
            .IsRequired();
            builder.Property(x => x.Aperture)
            .HasMaxLength(20)
            .HasColumnType("NVARCHAR")
            .IsRequired();
            builder.Property(x => x.ShutterSpeed)
            .HasMaxLength(20)
            .HasColumnType("NVARCHAR")
            .IsRequired();
            builder.Property(x => x.WhiteBalance)
            .HasMaxLength(20)
            .HasColumnType("NVARCHAR")
            .IsRequired();
        }
    }
}
