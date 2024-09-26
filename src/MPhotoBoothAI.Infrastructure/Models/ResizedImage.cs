
using Emgu.CV;

namespace MPhotoBoothAI.Infrastructure.Models;

public class ResizedImage(Mat image, int newh, int neww, int padh, int padw)
{
    public Mat Image { get; private set; } = image;
    public int Newh { get; private set; } = newh;
    public int Neww { get; private set; } = neww;
    public int Padh { get; private set; } = padh;
    public int Padw { get; private set; } = padw;
}
