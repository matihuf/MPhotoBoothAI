using Emgu.CV;

namespace MPhotoBoothAI.Models.FaceSwaps;
public record FaceSwapTemplate(string FilePath, int Faces, Mat MarkedFaces) : IDisposable
{
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            MarkedFaces?.Dispose();
        }
    }
}
