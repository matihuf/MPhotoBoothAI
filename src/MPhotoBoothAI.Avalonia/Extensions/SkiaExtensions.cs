using Avalonia.Media;
using MPhotoBoothAI.Avalonia.Models;
using SkiaSharp;

namespace MPhotoBoothAI.Avalonia.Extensions
{
    static class SkiaExtensions
    {
        public static IImage? ToAvaloniaImage(this SKBitmap? bitmap)
        {
            if (bitmap is not null)
            {
                return new AvaloniaImage(bitmap);
            }
            return default;
        }
    }
}
