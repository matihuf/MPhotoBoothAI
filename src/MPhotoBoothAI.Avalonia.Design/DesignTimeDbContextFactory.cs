using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Infrastructure.Persistence;
using System.Diagnostics;

namespace MPhotoBoothAI.Avalonia.Design;

internal static class DesignTimeDbContextFactory
{
    public static IDatabaseContext CreateDbContext()
    {
        var fvi = FileVersionInfo.GetVersionInfo(typeof(DesignTimeDbContextFactory).Assembly.Location);
        var builder = new DbContextOptionsBuilder<DatabaseContext>();
        builder.UseSqlite($"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), fvi.CompanyName, fvi.ProductName, $"{fvi.ProductName}.db")}");
        return new DatabaseContext(builder.Options);
    }
}
