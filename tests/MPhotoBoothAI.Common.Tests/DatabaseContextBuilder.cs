using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Infrastructure.Persistence;

namespace MPhotoBoothAI.Common.Tests;
public class DatabaseContextBuilder : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<DatabaseContext> _options;

    public DatabaseContextBuilder()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        _options = new DbContextOptionsBuilder<DatabaseContext>().UseSqlite(_connection).Options;
        using var db = new DatabaseContext(_options);
        db.Database.EnsureCreated();
    }

    public IDatabaseContext Build() => new DatabaseContext(_options);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }
    }
}
