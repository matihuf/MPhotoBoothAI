using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace MPhotoBoothAI.Infrastructure.Services;

public class FaceMaskService
{
    // Top of the eye arrays
    private readonly int[] _botLIndices = [35, 41, 40, 42, 39];
    private readonly int[] _botRIndices = [89, 95, 94, 96, 93];

    // Eyebrow arrays
    private readonly int[] _topLIndices = [43, 48, 49, 51, 50];
    private readonly int[] _topRIndices = [102, 103, 104, 105, 101];

    public Mat GetMask(Mat frame, float[,] frameLandmarks, float[,] targetLandmarks)
    {
        float left = (frameLandmarks[1, 0] - targetLandmarks[1, 0]) + (frameLandmarks[2, 0] - targetLandmarks[2, 0]) + (frameLandmarks[13, 0] - targetLandmarks[13, 0]);
        float right = (targetLandmarks[17, 0] - frameLandmarks[17, 0]) + (targetLandmarks[18, 0] - frameLandmarks[18, 0]) + (targetLandmarks[29, 0] - frameLandmarks[29, 0]);
        float offset = MathF.Max(left, right);
        int erode, sigmaX, sigmaY;
        if (offset > 6)
        {
            erode = 15;
            sigmaX = 15;
            sigmaY = 10;
        }
        else if (offset > 3)
        {
            erode = 10;
            sigmaX = 10;
            sigmaY = 8;
        }
        else if (offset < -3)
        {
            erode = -5;
            sigmaX = 5;
            sigmaY = 10;
        }
        else
        {
            erode = 5;
            sigmaX = 5;
            sigmaY = 5;
        }

        float eyebrows_expand_mod;
        if (erode == 15)
        {
            eyebrows_expand_mod = 2.7f;
        }
        else if (erode == -5)
        {
            eyebrows_expand_mod = 0.5f;
        }
        else
        {
            eyebrows_expand_mod = 2.0f;
        }
        var expandedLandmarks = ExpandEyebrows(frameLandmarks, eyebrows_expand_mod);
        using Mat mask = GetMask(frame, expandedLandmarks);
        using Mat erodeMask = ErodeAndBlur(mask, erode, sigmaX, sigmaY);
        var result = new Mat();
        erodeMask.ConvertTo(result, DepthType.Cv64F);
        return result;
    }

    private Mat ErodeAndBlur(Mat maskInput, int erode, int sigmaX, int sigmaY)
    {
        using var mask = new Mat();
        maskInput.ConvertTo(mask, DepthType.Cv8U);

        var kernelSize = erode > 0 ? new Size(erode, erode) : new Size(-erode, -erode);
        using var kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, kernelSize, new Point(-1, -1));
        using var erodeResult = new Mat();
        if (erode > 0)
        {
            CvInvoke.Erode(mask, erodeResult, kernel, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
        }
        else
        {
            CvInvoke.Dilate(mask, erodeResult, kernel, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
        }

        int clipSize = sigmaY * 2;
        CvInvoke.Rectangle(erodeResult, new Rectangle(0, 0, erodeResult.Cols, clipSize), CvInvoke.MorphologyDefaultBorderValue, -1);
        // Set bottom border to zero
        CvInvoke.Rectangle(erodeResult, new Rectangle(0, erodeResult.Rows - clipSize, erodeResult.Cols, clipSize), CvInvoke.MorphologyDefaultBorderValue, -1);
        // Set left border to zero
        CvInvoke.Rectangle(erodeResult, new Rectangle(0, 0, clipSize, erodeResult.Rows), CvInvoke.MorphologyDefaultBorderValue, -1);
        // Set right border to zero
        CvInvoke.Rectangle(erodeResult, new Rectangle(erodeResult.Cols - clipSize, 0, clipSize, erodeResult.Rows), CvInvoke.MorphologyDefaultBorderValue, -1);
    
        var gaussianBlur = new Mat();
        CvInvoke.GaussianBlur(erodeResult, gaussianBlur, new Size(0,0), sigmaX, sigmaY);

        return gaussianBlur;
    }


    private static Mat GetMask(Mat frame, int[,] landmarks)
    {
        using var imgGray = new Mat();
        CvInvoke.CvtColor(frame, imgGray, ColorConversion.Bgr2Gray);
        var mask = Mat.Zeros(imgGray.Rows, imgGray.Cols, DepthType.Cv8S, imgGray.NumberOfChannels);

        Point[] points = new Point[landmarks.GetLength(0)];
        for (int i = 0; i < landmarks.GetLength(0); i++)
        {
            points[i] = new Point(landmarks[i, 0], landmarks[i, 1]);
        }
        using var hullInput = new VectorOfPoint(points);
        using var hull = new Mat();
        CvInvoke.ConvexHull(hullInput, hull);
        CvInvoke.FillConvexPoly(mask, hull, new MCvScalar(255));
        return mask;
    }

    private int[,] ExpandEyebrows(float[,] lmrks, float eyebrowsExpandMod)
    {
        int[,] newLmrks = new int[lmrks.GetLength(0), lmrks.GetLength(1)];
        for (int i = 0; i < lmrks.GetLength(0); i++)
        {
            for (int j = 0; j < lmrks.GetLength(1); j++)
            {
                newLmrks[i, j] = (int)lmrks[i, j];
            }
        }

        // Adjust eyebrow arrays
        for (int i = 0; i < 5; i++)
        {
            // Calculate the new positions for the left eyebrow
            newLmrks[_topLIndices[i], 0] = (int)(lmrks[_topLIndices[i], 0] + eyebrowsExpandMod * 0.5f * (lmrks[_topLIndices[i], 0] - lmrks[_botLIndices[i], 0]));
            newLmrks[_topLIndices[i], 1] = (int)(lmrks[_topLIndices[i], 1] + eyebrowsExpandMod * 0.5f * (lmrks[_topLIndices[i], 1] - lmrks[_botLIndices[i], 1]));

            // Calculate the new positions for the right eyebrow
            newLmrks[_topRIndices[i], 0] = (int)(lmrks[_topRIndices[i], 0] + eyebrowsExpandMod * 0.5f * (lmrks[_topRIndices[i], 0] - lmrks[_botRIndices[i], 0]));
            newLmrks[_topRIndices[i], 1] = (int)(lmrks[_topRIndices[i], 1] + eyebrowsExpandMod * 0.5f * (lmrks[_topRIndices[i], 1] - lmrks[_botRIndices[i], 1]));
        }

        return newLmrks;
    }
}
