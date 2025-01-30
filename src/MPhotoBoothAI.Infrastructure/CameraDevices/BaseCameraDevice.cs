using Emgu.CV;
using Microsoft.Extensions.Logging;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Infrastructure.CameraDevices;

public abstract class BaseCameraDevice(ILogger<BaseCameraDevice> logger)
{
    private readonly ILogger<BaseCameraDevice> _logger = logger;
    private readonly List<IObserver> _observers = [];

    public void Attach(IObserver observer) => _observers.Add(observer);

    public void Detach(IObserver observer) => _observers.Remove(observer);

    public void Notify(Mat mat)
    {
        try
        {
            foreach (var observer in _observers.ToList())
            {
                try
                {
                    if (mat != null && !mat.IsEmpty)
                    {
                        observer.Notify(mat.Clone());
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Notify failed for {Observer}", observer.GetType().Name);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Notify failed");
        }
        finally
        {
            mat?.Dispose();
        }
    }
}
