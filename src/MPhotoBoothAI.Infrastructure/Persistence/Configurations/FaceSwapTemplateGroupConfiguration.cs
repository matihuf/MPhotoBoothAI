using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Infrastructure.Persistence.Configurations;
public class FaceSwapTemplateGroupConfiguration : IEntityTypeConfiguration<FaceSwapTemplateGroupEntity>
{
    public void Configure(EntityTypeBuilder<FaceSwapTemplateGroupEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .HasColumnType("NVARCHAR")
            .IsRequired();
        builder.HasMany(x => x.Templates)
            .WithOne(x => x.FaceSwapTemplateGroup)
            .HasForeignKey(b => b.FaceSwapTemplateGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
