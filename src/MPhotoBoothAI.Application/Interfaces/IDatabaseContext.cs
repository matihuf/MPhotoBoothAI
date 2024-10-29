using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IDatabaseContext : IDisposable
{
    public DbSet<UserSettingsEntity> UserSettings { get; }
    public DatabaseFacade Database { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}
