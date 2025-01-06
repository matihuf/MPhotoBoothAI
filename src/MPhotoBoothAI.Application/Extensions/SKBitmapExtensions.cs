using Emgu.CV;
using Emgu.CV.CvEnum;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace MPhotoBoothAI.Application.Extensions;
public static class SKBitmapExtensions
{
    public static Mat ToMat(this SKBitmap? bitmap)
    {
        if (bitmap is null)
        {
            return new();
        }

        int width = bitmap.Width;
        int height = bitmap.Height;
        Mat mat;
        switch (bitmap.ColorType)
        {
            case SKColorType.Bgra8888:
                mat = new Mat(height, width, DepthType.Cv8U, 4);
                break;

            case SKColorType.Rgba8888:
                bitmap = bitmap.Copy(SKColorType.Bgra8888);
                mat = new Mat(height, width, DepthType.Cv8U, 4);
                break;

            case SKColorType.Rgb888x:
                mat = new Mat(height, width, DepthType.Cv8U, 4);
                break;

            case SKColorType.Rgb565:
                bitmap = bitmap.Copy(SKColorType.Rgb565);
                mat = new Mat(height, width, DepthType.Cv8U, 4);
                break;
            default:
                throw new NotSupportedException("Unhandled color type SKBitmap.");
        }
        SKPixmap pixmap = bitmap.PeekPixels() ?? throw new Exception("Can't pick pixels from SKBitmap.");
        int size = pixmap.RowBytes * height;
        byte[] pixelData = new byte[size];
        Marshal.Copy(pixmap.GetPixels(), pixelData, 0, size);
        mat.SetTo(pixelData);
        if (bitmap.ColorType == SKColorType.Rgb888x)
        {
            Mat mat3Channel = new();
            CvInvoke.CvtColor(mat, mat3Channel, ColorConversion.Bgr2Bgra);
            mat.Dispose();
            return mat3Channel;
        }
        return mat;
    }

    public static void DrawRoundedRectangle(this SKBitmap image, SKRect box, int cornerRadius, SKColor color, int thickness = 1)
    {
        using var canvas = new SKCanvas(image);
        using var paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = color,
            StrokeWidth = thickness,
            IsAntialias = true
        };

        using var path = new SKPath();
        path.AddRoundRect(box, cornerRadius, cornerRadius);
        canvas.DrawPath(path, paint);
    }
}
