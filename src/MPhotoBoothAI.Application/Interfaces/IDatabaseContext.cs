using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IDatabaseContext : IDisposable
{
    public DbSet<UserSettingsEntity> UserSettings { get; set; }
    public DbSet<CameraSettingsEntity> CameraSettings { get; set; }
    public DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}
