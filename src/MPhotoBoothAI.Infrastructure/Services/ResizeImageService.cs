using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models;
using System.Drawing;

namespace MPhotoBoothAI.Infrastructure.Services;

public class ResizeImageService : IResizeImageService
{
    private readonly MCvScalar _border = new(0, 0, 0, 0);

    public ResizedImage Resize(Mat frame, int inputHeight = 640, int inputWidth = 640, bool keepRatio = true)
    {
        int srch = frame.Rows, srcw = frame.Cols;
        int newh = 0, neww = 0, top = 0, left = 0;
        var result = new Mat();
        using var resized = new Mat();
        if (keepRatio && srch != srcw)
        {
            float hwScale = (float)srch / srcw;
            if (hwScale > 1)
            {
                newh = inputHeight;
                neww = (int)(inputWidth / hwScale);
                CvInvoke.Resize(frame, resized, new Size(neww, newh), interpolation: Inter.Area);
                left = (int)((inputWidth - neww) * 0.5);
                CvInvoke.CopyMakeBorder(resized, result, 0, 0, left, inputWidth - neww - left, BorderType.Constant, _border);
            }
            else
            {
                newh = (int)(inputHeight * hwScale);
                neww = inputWidth;
                CvInvoke.Resize(frame, resized, new Size(neww, newh), interpolation: Inter.Area);
                top = (int)((inputHeight - newh) * 0.5);
                CvInvoke.CopyMakeBorder(resized, result, top, inputHeight - newh - top, 0, 0, BorderType.Constant, _border);
            }
        }
        else
        {
            CvInvoke.Resize(frame, result, new Size(inputWidth, inputHeight), interpolation: Inter.Area);
        }
        return new ResizedImage(result, newh, neww, top, left);
    }

    public Mat GetThumbnail(Mat frame, float percentage)
    {
        int inputHeight = frame.Height - (int)Math.Round(frame.Height * percentage);
        int inputWidth = frame.Width - (int)Math.Round(frame.Width * percentage);
        var resized = new Mat();
        CvInvoke.Resize(frame, resized, new Size(inputWidth, inputHeight), interpolation: Inter.Area);
        return resized;
    }
}
