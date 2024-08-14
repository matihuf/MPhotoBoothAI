using Emgu.CV;

namespace MPhotoBoothAI.Application.Models;

public class FaceAlign(Mat norm, Mat align) : IDisposable
{
    public Mat Norm { get; private set; } = norm;
    public Mat Align { get; private set; } = align;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Norm?.Dispose();
            Align?.Dispose();
        }
    }
}
