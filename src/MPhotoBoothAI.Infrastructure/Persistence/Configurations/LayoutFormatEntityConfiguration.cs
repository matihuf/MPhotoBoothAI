using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Infrastructure.Persistence.Configurations;
public class LayoutFormatEntityConfiguration : IEntityTypeConfiguration<LayoutFormatEntity>
{
    public void Configure(EntityTypeBuilder<LayoutFormatEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SizeName).HasColumnType("NVARCHAR").HasMaxLength(20).IsRequired();
        builder.Property(x => x.FormatWidth).HasColumnType("REAL").IsRequired();
        builder.Property(x => x.FormatHeight).HasColumnType("REAL").IsRequired();
        builder.Property(x => x.FormatRatio).HasColumnType("REAL").IsRequired();

        builder.HasData(new LayoutFormatEntity { Id = 1, SizeName = "5x15", FormatWidth = 3000, FormatHeight = 1000, FormatRatio = 1d / 3 });
        builder.HasData(new LayoutFormatEntity { Id = 2, SizeName = "10x15", FormatWidth = 3000, FormatHeight = 2000, FormatRatio = 2d / 3 });
    }
}
