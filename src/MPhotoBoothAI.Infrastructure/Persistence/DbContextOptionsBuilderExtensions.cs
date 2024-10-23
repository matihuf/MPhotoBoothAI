using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MPhotoBoothAI.Infrastructure.Services;

namespace MPhotoBoothAI.Infrastructure.Persistence;
public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseSqlite(this DbContextOptionsBuilder optionsBuilder)
    {
        var applicationInfoService = new ApplicationInfoService();
        optionsBuilder.UseSqlite($"Data Source={Path.Combine(applicationInfoService.UserProfilePath, $"{applicationInfoService.Product}.db")}", sql =>
        {
            sql.MigrationsHistoryTable(HistoryRepository.DefaultTableName);
        });
        return optionsBuilder;
    }
}
