using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Infrastructure.Persistence.Configurations;
public class UserSettingsEntityConfiguration : IEntityTypeConfiguration<UserSettingsEntity>
{
    public void Configure(EntityTypeBuilder<UserSettingsEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CultureInfoName)
            .HasMaxLength(5)
            .HasColumnType("NVARCHAR")
            .IsRequired();
        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME")
            .HasDefaultValueSql("date('now')")
            .IsRequired();
    }
}
