
using Emgu.CV;

namespace MPhotoBoothAI.Models;

public class ResizedImage(Mat image, int newh, int neww, int padh, int padw) : IDisposable
{
    public Mat Image { get; private set; } = image;
    public int Newh { get; private set; } = newh;
    public int Neww { get; private set; } = neww;
    public int Padh { get; private set; } = padh;
    public int Padw { get; private set; } = padw;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Image?.Dispose();
        }
    }
}
