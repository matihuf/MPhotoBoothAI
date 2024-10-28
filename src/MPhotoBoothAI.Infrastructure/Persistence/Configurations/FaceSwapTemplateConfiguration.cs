using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Infrastructure.Persistence.Configurations;
public class FaceSwapTemplateConfiguration : IEntityTypeConfiguration<FaceSwapTemplateEntity>
{
    public void Configure(EntityTypeBuilder<FaceSwapTemplateEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FileName)
            .HasMaxLength(50)
            .HasColumnType("NVARCHAR")
            .IsRequired();
    }
}
