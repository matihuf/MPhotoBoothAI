using System.Drawing;
using Emgu.CV.Util;

namespace MPhotoBoothAI.Application.Models;

public class FaceDetection(Rectangle box, float confidence, VectorOfPointF landmarks) : IDisposable
{
    public Rectangle Box { get; private set; } = box;
    public float Confidence { get; private set; } = confidence;
    public VectorOfPointF Landmarks { get; private set; } = landmarks;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Landmarks?.Dispose();
        }
    }
}
