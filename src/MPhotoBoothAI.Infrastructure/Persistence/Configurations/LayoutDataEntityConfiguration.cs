using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Infrastructure.Persistence.Configurations;
public class LayoutDataEntityConfiguration : IEntityTypeConfiguration<LayoutDataEntity>
{
    public void Configure(EntityTypeBuilder<LayoutDataEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.PhotoLayoutData)
            .WithOne(x => x.LayoutData)
            .HasForeignKey(x => x.LayoutDataEntityId);
        builder.HasMany(x => x.OverlayImageData)
            .WithOne(x => x.LayoutData)
            .HasForeignKey(x => x.LayoutDataEntityId);

        builder.HasData(new LayoutDataEntity { Id = 1, PhotoLayoutData = [], OverlayImageData = [] });
        builder.HasData(new LayoutDataEntity { Id = 2, PhotoLayoutData = [], OverlayImageData = [] });
    }
}
