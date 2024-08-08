using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace MPhotoBoothAI.Infrastructure.Services;

public class FaceAlignService()
{
    private readonly Size _cropSize = new(224, 224);

    /// <summary>
    /// Magic numbers, face profiles
    /// </summary>
    private static readonly PointF[][] _src =
    [
        [
            new PointF(51.642f, 50.115f), new PointF(57.617f, 49.990f), new PointF(35.740f, 69.007f),
            new PointF(51.157f, 89.050f), new PointF(57.025f, 89.702f)
        ],
        [
            new PointF(45.031f, 50.118f), new PointF(65.568f, 50.872f), new PointF(39.677f, 68.111f),
            new PointF(45.177f, 86.190f), new PointF(64.246f, 86.758f)
        ],
        [
            new PointF(39.730f, 51.138f), new PointF(72.270f, 51.138f), new PointF(56.000f, 68.493f),
            new PointF(42.463f, 87.010f), new PointF(69.537f, 87.010f)
        ],
        [
            new PointF(46.845f, 50.872f), new PointF(67.382f, 50.118f), new PointF(72.737f, 68.111f),
            new PointF(48.167f, 86.758f), new PointF(67.236f, 86.190f)
        ],
        [
            new PointF(54.796f, 49.990f), new PointF(60.771f, 50.115f), new PointF(76.673f, 69.007f),
            new PointF(55.388f, 89.702f), new PointF(61.257f, 89.050f)
        ]
    ];

    private static readonly Dictionary<int, PointF[][]> srcMap = new()
    {
        { 112, _src },
        { 224, _src.Select(pts => pts.Select(p => new PointF(p.X * 2, p.Y * 2)).ToArray()).ToArray() }
    };

    private static readonly PointF[] arcfaceSrc =
    [
        new(38.2946f, 51.6963f), new(73.5318f, 51.5014f), new(56.0252f, 71.7366f),
        new(41.5493f, 92.3655f), new(70.7299f, 92.2041f)
    ];

    public Mat Align(Mat frame, VectorOfPointF landmarks)
    {
        using var affinePartial2D = EstimateNorm(landmarks, _cropSize.Width, string.Empty);
        using var affinePartial2DFirstRow = affinePartial2D.Row(0);
        var dst = new Mat();
        CvInvoke.WarpAffine(frame, dst, affinePartial2D, _cropSize);
        return dst;
    }

    private static Mat EstimateNorm(VectorOfPointF landmarks, int imageSize = 112, string mode = "arcface")
    {
        var minM = new Mat();
        double minError = double.MaxValue;
        var transformedLandmarks = TransformLandmarks(landmarks);
        PointF[][] srcPts;
        if (mode == "arcface")
        {
            srcPts = [arcfaceSrc];
        }
        else
        {
            srcMap.TryGetValue(imageSize, out srcPts);
        }
        foreach (var src in srcPts)
        {
            using var srcVector = new VectorOfPointF(src);
            using var estimation = CvInvoke.EstimateAffinePartial2D(landmarks, srcVector, null, RobustEstimationAlgorithm.LMEDS, 100, 2000, 0.99, 10);
            using var resultDot = Multiply(estimation, transformedLandmarks);
            using var resultDotTransposed = new Matrix<double>(5, 2);
            CvInvoke.Transpose(resultDot, resultDotTransposed);
            var error = CalculateError(resultDotTransposed, src);
            if (error < minError)
            {
                minError = error;
                minM = estimation.Clone();
            }
        }
        return minM;
    }

    private static double CalculateError(Matrix<double> results, PointF[] src)
    {
        double error = 0.0;
        for (int i = 0; i < results.Rows; i++)
        {
            double diffX = results[i, 0] - src[i].X;
            double diffY = results[i, 1] - src[i].Y;
            double sum = diffX * diffX + diffY * diffY;
            error += Math.Sqrt(sum);
        }
        return error;
    }

    private static Matrix<double> Multiply(Mat estimation, double[,] transformedLandmarks)
    {
        var resultDot = new Matrix<double>(2, 5);
        var estimationArray = (double[,])estimation.GetData();
        for (int m = 0; m < 2; m++)
        {
            for (int j = 0; j < 5; j++)
            {
                resultDot[m, j] = 0;
                for (int k = 0; k < 3; k++)
                {
                    resultDot[m, j] += estimationArray[m, k] * transformedLandmarks[k, j];
                }
            }
        }
        return resultDot;
    }

    private static double[,] TransformLandmarks(VectorOfPointF landmarks)
    {
        using var lm_tran = new Mat(3, 5, DepthType.Cv64F, 1);
        var transformedLandmarks = (double[,])lm_tran.GetData();
        for (int i = 0; i < 5; i++)
        {
            transformedLandmarks[0, i] = landmarks[i].X;
            transformedLandmarks[1, i] = landmarks[i].Y;
            transformedLandmarks[2, i] = 1;
        }
        return transformedLandmarks;
    }
}
