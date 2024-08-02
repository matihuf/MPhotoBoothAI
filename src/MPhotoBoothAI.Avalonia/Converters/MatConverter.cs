using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MPhotoBoothAI.Avalonia.Converters;

public class MatConverter : IValueConverter
{
    public static MatConverter Instance = new MatConverter();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }
        return null; //ConvertToAvaloniaBitmap(((Mat)value).ToBitmap());
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    // private static Bitmap ConvertToAvaloniaBitmap(System.Drawing.Bitmap bitmap)
    // {
    //     if (bitmap == null)
    //     {
    //         return null;
    //     }
    //     System.Drawing.Bitmap bitmapTmp = new System.Drawing.Bitmap(bitmap);
    //     var bitmapdata = bitmapTmp.LockBits(new System.Drawing.Rectangle(0, 0, bitmapTmp.Width, bitmapTmp.Height),
    //         System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

    //     Bitmap bitmap1 = new Bitmap(PixelFormat.Bgra8888, AlphaFormat.Unpremul,
    //         bitmapdata.Scan0,
    //         new PixelSize(bitmapdata.Width, bitmapdata.Height),
    //         new Vector(96, 96),
    //         bitmapdata.Stride);
    //     bitmapTmp.UnlockBits(bitmapdata);
    //     bitmapTmp.Dispose();
    //     return bitmap1;
    // }
}
