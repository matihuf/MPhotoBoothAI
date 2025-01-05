using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Infrastructure.Persistence;
public class DatabaseContext : DbContext, IDatabaseContext
{
    public DbSet<UserSettingsEntity> UserSettings { get; set; }
    public DbSet<FaceSwapTemplateGroupEntity> FaceSwapTemplateGroups { get; set; }
    public DbSet<FaceSwapTemplateEntity> FaceSwapTemplates { get; set; }

    public DbSet<CameraSettingsEntity> CameraSettings { get; set; }

    public DbSet<PhotoLayoutDataEntity> PhotosLayoutData { get; set; }
    public DbSet<OverlayImageDataEnitity> OverlayImagesData { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}
