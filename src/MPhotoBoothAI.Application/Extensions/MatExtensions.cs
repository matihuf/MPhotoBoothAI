using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using SkiaSharp;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MPhotoBoothAI.Application.Extensions;
public static class MatExtensions
{
    public static SKBitmap ToSKBitmap(this Mat mat)
    {
        int width = mat.Width;
        int height = mat.Height;
        int channels = mat.NumberOfChannels;
        SKColorType colorType;
        SKAlphaType alphaType = SKAlphaType.Premul;
        if (mat.Depth != DepthType.Cv8U)
        {
            throw new NotSupportedException("Mat must have depth Cv8U.");
        }
        if (channels == 4)
        {
            colorType = SKColorType.Bgra8888;
        }
        else if (channels == 3)
        {
            colorType = SKColorType.Bgra8888;
            alphaType = SKAlphaType.Opaque;
        }
        else
        {
            throw new NotSupportedException("Mat must have 3 or 4 channels.");
        }
        SKImageInfo info = new SKImageInfo(width, height, colorType, alphaType);
        SKBitmap skBitmap = new SKBitmap(info);

        int size = width * height * info.BytesPerPixel;

        byte[] matData = new byte[size];

        if (channels == 3)
        {
            using var mat4Channel = new Mat();
            CvInvoke.CvtColor(mat, mat4Channel, ColorConversion.Bgr2Bgra);
            mat4Channel.CopyTo(matData);
        }
        else
        {
            mat.CopyTo(matData);
        }
        Marshal.Copy(matData, 0, skBitmap.GetPixels(), size);
        return skBitmap;
    }

    public static void DrawRoundedRectangle(this Mat frame, Rectangle box, int cornerRadius, MCvScalar color, int thickness = 1)
    {
        using var image = frame.ToSKBitmap();
        image.DrawRoundedRectangle(new SKRect(box.Left, box.Top, box.Right, box.Bottom), cornerRadius, new SKColor((byte)color.V2, (byte)color.V1, (byte)color.V0), thickness);
        using var tmpMat = image.ToMat();
        tmpMat.CopyTo(frame);
    }
}
