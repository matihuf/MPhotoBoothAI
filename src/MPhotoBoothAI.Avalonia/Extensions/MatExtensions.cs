﻿using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Emgu.CV;
using System;
using System.Threading.Tasks;

namespace MPhotoBoothAI.Avalonia;

public static class MatExtensions
{
    public static void ToBitmapParallel(this Mat mat, WriteableBitmap dst)
    {
        ArgumentNullException.ThrowIfNull(mat);
        ArgumentNullException.ThrowIfNull(dst);

        if (mat.Width != dst.PixelSize.Width || mat.Height != dst.PixelSize.Height)
            throw new ArgumentException("size of src must be equal to size of dst");
        if (dst.Format != PixelFormat.Bgra8888 || !dst.Dpi.Equals(new Vector(96, 96)))
            throw new ArgumentException("Currently only Bgra8888 + 96 DPI WriteableBitmaps can be reused ");


        var width = mat.Width;
        var height = mat.Height;

        using var lockBuffer = dst.Lock();
        var stride = lockBuffer.RowBytes;
        var bufferAddress = lockBuffer.Address;
        unsafe
        {
            switch (mat.NumberOfChannels)
            {
                // Method 1 (Span copy)
                case 1:
                    Parallel.For(0, height, y =>
                    {
                        var spanBitmap = new Span<uint>(IntPtr.Add(bufferAddress, y * stride).ToPointer(), width); // lockBuffer.GetPixelRowSpan(y,width);
                        var spanMat = mat.GetRowSpan<byte>(y, width);
                        for (var x = 0; x < width; x++)
                        {
                            var color = spanMat[x];
                            spanBitmap[x] = (uint)(color | (color << 8) | (color << 16) | (0xff << 24));
                        }
                    });
                    break;
                case 3:
                    Parallel.For(0, height, y =>
                    {
                        var spanBitmap = new Span<uint>(IntPtr.Add(bufferAddress, y * stride).ToPointer(), width); // lockBuffer.GetPixelRowSpan(y,width);
                        var spanMat = mat.GetRowSpan<byte>(y);
                        var pixel = 0;
                        for (var x = 0; x < width; x++) spanBitmap[x] = (uint)(spanMat[pixel++] | (spanMat[pixel++] << 8) | (spanMat[pixel++] << 16) | (0xff << 24));
                    });

                    break;
                case 4:
                    if (mat.Depth == Emgu.CV.CvEnum.DepthType.Cv8U)
                    {
                        Parallel.For(0, height, y =>
                        {
                            var spanBitmap = new Span<uint>(IntPtr.Add(bufferAddress, y * stride).ToPointer(), width); // lockBuffer.GetPixelRowSpan(y,width);
                            var spanMat = mat.GetRowSpan<byte>(y);
                            var pixel = 0;
                            for (var x = 0; x < width; x++) spanBitmap[x] = (uint)(spanMat[pixel++] | (spanMat[pixel++] << 8) | (spanMat[pixel++] << 16) | (spanMat[pixel++] << 24));
                        });
                    }
                    break;
            }
        }
    }

    public static unsafe Span<T> GetRowSpan<T>(this Mat mat, int y, int length = 0, int offset = 0)
      => new(IntPtr.Add(mat.DataPointer, y * mat.GetRealStep() + offset).ToPointer(), length <= 0 ? mat.GetRealStep() : length);

    public static int GetRealStep(this Mat mat)
      => mat.Width * mat.NumberOfChannels;
}
