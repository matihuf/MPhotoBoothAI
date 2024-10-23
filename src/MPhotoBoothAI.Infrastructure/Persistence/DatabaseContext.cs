using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Infrastructure.Persistence;
public class DatabaseContext : DbContext, IDatabaseContext
{
    public DbSet<UserSettingsEntity> UserSettings { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        modelBuilder.Entity<UserSettingsEntity>().HasData(new UserSettingsEntity { Id = 1, CultureInfoName = Thread.CurrentThread.CurrentUICulture.Name });
    }
}
