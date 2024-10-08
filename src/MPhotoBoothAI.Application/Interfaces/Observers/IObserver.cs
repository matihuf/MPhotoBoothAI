using SkiaSharp;

namespace MPhotoBoothAI.Application.Interfaces.Observers;

public interface IObserver
{
    /// <summary>
    /// Remeber to dispose Mat
    /// </summary>
    /// <param name="mat"></param>
    void Notify(SKBitmap bitmap);
}
