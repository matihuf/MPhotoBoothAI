using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Util;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.Services.Swap;

public class FaceSwapPredictService([FromKeyedServices(Consts.AiModels.ArcfaceBackbone)] Net arfaceNet, [FromKeyedServices(Consts.AiModels.Gunet2blocks)] Net gNet) : IFaceSwapPredictService
{
    private readonly Net _arfaceNet = arfaceNet;
    private readonly Net _gNet = gNet;
    private const float _normalizeFactor = 127.5f;

    public Mat Predict(Mat sourceFace, Mat targetFace)
    {
        using var sourceFaceHalf = HalfSize(sourceFace);
        using var sourceBlob = Preprocess(sourceFaceHalf);
        _arfaceNet.SetInput(sourceBlob, "img");
        using var arfaceNetOuts = new VectorOfMat();
        _arfaceNet.Forward(arfaceNetOuts, _arfaceNet.UnconnectedOutLayersNames);

        using var resizedTarget = new Mat();
        CvInvoke.Resize(targetFace, resizedTarget, new Size(256, 256));
        using var targetBlob = Preprocess(resizedTarget);
        _gNet.SetInput(arfaceNetOuts[0], "source_emb");
        _gNet.SetInput(targetBlob, "target");
        using var gOuts = new VectorOfMat();
        _gNet.Forward(gOuts, _gNet.UnconnectedOutLayersNames);
        using var image = ConvertToImage(gOuts[0]);
        var originalImageSize = new Mat();
        CvInvoke.Resize(image, originalImageSize, sourceFace.Size);
        return originalImageSize;
    }

    public static Mat ConvertToImage(Mat gOut)
    {
        var data = (float[,,,])gOut.GetData();
        int channels = data.GetLength(1);
        int height = data.GetLength(2);
        int width = data.GetLength(3);

        byte[,,] result = new byte[width, height, channels];
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                for (int c = 0; c < channels; c++)
                {
                    result[w, h, c] = (byte)Denormalize(data[0, c, w, h]);
                }
            }
        }
        GCHandle handle = GCHandle.Alloc(result, GCHandleType.Pinned);
        var image = new Mat(256, 256, DepthType.Cv8U, 3, handle.AddrOfPinnedObject(), 0);
        handle.Free();
        return image;
    }

    private static float Denormalize(float normalized) => normalized * _normalizeFactor + _normalizeFactor;

    private static Mat Preprocess(Mat face)
    {
        using var normalized = Normalize(face);
        return DnnInvoke.BlobFromImage(normalized, 1, face.Size);
    }

    private static Mat HalfSize(Mat face)
    {
        var halfSize = new Mat();
        var size = new Size(face.Width / 2, face.Height / 2);
        CvInvoke.Resize(face, halfSize, size);
        return halfSize;
    }

    private static Mat Normalize(Mat face)
    {
        var normalizedImg = new Mat();
        face.ConvertTo(normalizedImg, DepthType.Cv32F, 1.0 / _normalizeFactor, -1.0);
        return normalizedImg;
    }
}
