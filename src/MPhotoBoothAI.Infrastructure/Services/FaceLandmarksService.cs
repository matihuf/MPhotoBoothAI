using System.Drawing;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Util;

namespace MPhotoBoothAI.Infrastructure.Services;

public class FaceLandmarksService(Net lNet)
{
    private readonly Net _lNet = lNet;

    /// <summary>
    /// Magic numbers
    /// </summary>
    private readonly Matrix<float> _m = new(new float[,]
    {
        { 0.57142857f, 0f, 32f },
        { 0f, 0.57142857f, 32f }
    });

    /// <summary>
    /// Magic numbers
    /// </summary>
    private readonly Matrix<float> _im = new(new float[,]
    {
            { 1.75f, -0f, -56f },
            { -0f, 1.75f, -56f }
    });

    public float[,] GetLandmarks(Mat frame)
    {
        var imageSize = new Size(192, 192);
        using var rimg = new Mat();
        CvInvoke.WarpAffine(frame, rimg, _m, imageSize);
        using var blob = DnnInvoke.BlobFromImage(rimg, 1, rimg.Size);
        _lNet.SetInput(blob, "data");
        using var gOuts = new VectorOfMat();
        _lNet.Forward(gOuts, _lNet.UnconnectedOutLayersNames);
        var postProcessed = PostProcess(gOuts[0], imageSize);
        return MultiplyMatrixAndVector(_im, postProcessed);
    }

    private static float[,] PostProcess(Mat outPut, Size imageSize)
    {
        var outPutData = ((float[,])outPut.GetData());
        int numPoints = outPutData.GetLength(1) / 2;
        var result = new float[numPoints, 3];
        for (int i = 0; i < 106; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                int index = i * 2 + j;
                result[i, j] = (outPutData[0, index] + 1) * (imageSize.Width / 2);
            }
            result[i, 2] = 1;
        }
        return result;
    }

    private static float[,] MultiplyMatrixAndVector(Matrix<float> matrix, float[,] vectors)
    {
        int rowsM = matrix.Rows;
        int colsM = matrix.Cols;
        int rowsV = vectors.GetLength(0);

        float[,] result = new float[rowsV, rowsM];
        for (int k = 0; k < rowsV; k++)
        {
            for (int i = 0; i < rowsM; i++)
            {
                result[k, i] = 0;
                for (int j = 0; j < colsM; j++)
                {
                    result[k, i] += matrix[i, j] * vectors[k, j];
                }
            }
        }
        return result;
    }
}