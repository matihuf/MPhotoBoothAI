namespace MPhotoBoothAI.Application.Interfaces;
public interface IDatabaseContext : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    void Migrate();
}
