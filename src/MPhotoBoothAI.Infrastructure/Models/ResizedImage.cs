
using Emgu.CV;

namespace MPhotoBoothAI.Infrastructure.Models;

public class ResizedImage(Mat image, int newh, int neww, int top, int left)
{
    public Mat Image { get; private set; } = image;
    public int Newh { get; private set; } = newh;
    public int Neww { get; private set; } = neww;
    public int Top { get; private set; } = top;
    public int Left { get; private set; } = left;
}
