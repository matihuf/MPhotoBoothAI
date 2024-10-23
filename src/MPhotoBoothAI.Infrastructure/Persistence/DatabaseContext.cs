using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.Persistence;
public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public void Migrate() => Database.Migrate();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Consts.Db.Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}
