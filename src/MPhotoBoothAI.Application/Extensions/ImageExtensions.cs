using Emgu.CV;
using Emgu.CV.CvEnum;
using SkiaSharp;

namespace MPhotoBoothAI.Application.Extensions
{
    public static class ImageExtensions
    {
        public static SKBitmap? ToSKBitmap(this System.IO.Stream? stream)
        {
            if (stream == null)
                return null;
            return SKBitmap.Decode(stream);
        }

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
                    bitmap = bitmap.Copy(SKColorType.Bgra8888);
                    mat = new Mat(height, width, DepthType.Cv8U, 4);
                    break;
                default:
                    throw new NotSupportedException("Unhandled color type SKBitmap.");
            }
            SKPixmap pixmap = bitmap.PeekPixels() ?? throw new Exception("Can't pick pixels from SKBitmap.");
            int size = pixmap.RowBytes * height;
            byte[] pixelData = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(pixmap.GetPixels(), pixelData, 0, size);
            mat.SetTo(pixelData);
            if (bitmap.ColorType == SKColorType.Rgb888x)
            {
                Mat mat3Channel = new();
                CvInvoke.CvtColor(mat, mat3Channel, ColorConversion.Bgr2Bgra);
                return mat3Channel;
            }
            return mat;
        }

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
                Mat mat4Channel = new Mat();
                CvInvoke.CvtColor(mat, mat4Channel, ColorConversion.Bgr2Bgra);
                mat4Channel.CopyTo(matData);
            }
            else
            {
                mat.CopyTo(matData);
            }
            System.Runtime.InteropServices.Marshal.Copy(matData, 0, skBitmap.GetPixels(), size);
            return skBitmap;
        }
    }
}
