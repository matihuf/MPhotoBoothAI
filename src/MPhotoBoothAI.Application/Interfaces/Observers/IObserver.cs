using Emgu.CV;

namespace MPhotoBoothAI.Application;

public interface IObserver
{
    /// <summary>
    /// Remeber to dispose Mat
    /// </summary>
    /// <param name="mat"></param>
    void Notify(Mat mat);
}
