using Emgu.CV;
using Emgu.CV.CvEnum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MPhotoBoothAI.Infrastructure.Services;

public class FaceEnhancerService([FromKeyedServices(Consts.AiModels.Gfpgan)] LazyDisposal<InferenceSession> gfpgan) : IFaceEnhancerService
{
    private readonly LazyDisposal<InferenceSession> _gfpgan = gfpgan;

    public Mat Enhance(Mat face)
    {
        using var resizedTarget = new Mat();
        CvInvoke.Resize(face, resizedTarget, new Size(512, 512));

        var denseTensor = new DenseTensor<float>(Preproces(resizedTarget), [1, resizedTarget.NumberOfChannels, resizedTarget.Height, resizedTarget.Width]);
        var inputs = new List<NamedOnnxValue>(1) { NamedOnnxValue.CreateFromTensor(_gfpgan.Value.InputMetadata.Keys.First(), denseTensor) };
        using var results = _gfpgan.Value.Run(inputs);
        using var postprocess = Postprocess(results.First().AsTensor<float>().ToArray(), resizedTarget.Height, resizedTarget.Width, resizedTarget.NumberOfChannels);
        var resizedEnhanced = new Mat();
        CvInvoke.Resize(postprocess, resizedEnhanced, face.Size);
        return resizedEnhanced;
    }

    public static Mat Postprocess(float[] img, int height, int width, int channels)
    {
        float[] deTransposed = DeTranspose(img, height, width, channels);
        byte[] deNormalized = DeNormalize(height, width, channels, deTransposed);

        GCHandle handle = GCHandle.Alloc(deNormalized, GCHandleType.Pinned);
        var image = new Mat(height, width, DepthType.Cv8U, channels, handle.AddrOfPinnedObject(), 0);
        handle.Free();
        return image;
    }

    private static byte[] DeNormalize(int height, int width, int channels, float[] transposedImg)
    {
        byte[] processedImg = new byte[height * width * channels];
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                int rIndex = (h * width * channels) + (w * channels);
                int gIndex = rIndex + 1;
                int bIndex = rIndex + 2;

                byte r = (byte)Math.Max(0, Math.Min(255, transposedImg[rIndex]));
                byte g = (byte)Math.Max(0, Math.Min(255, transposedImg[gIndex]));
                byte b = (byte)Math.Max(0, Math.Min(255, transposedImg[bIndex]));

                processedImg[rIndex] = r;
                processedImg[gIndex] = g;
                processedImg[bIndex] = b;
            }
        }
        return processedImg;
    }

    private static float[] DeTranspose(float[] img, int height, int width, int channels)
    {
        float[] transposedImg = new float[img.Length];
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                for (int c = 0; c < channels; c++)
                {
                    int oldIndex = (c * height * width) + h * width + w;
                    int newIndex = (h * width * channels) + w * channels + c;
                    float value = Math.Max(-1.0f, Math.Min(1.0f, img[oldIndex]));
                    transposedImg[newIndex] = (value + 1.0f) * 0.5f * 255;
                }
            }
        }
        return transposedImg;
    }

    private static float[] Preproces(Mat mat)
    {
        float[] normalized = Normalize(mat);
        return TransposeImage(normalized, mat.Rows, mat.Cols, mat.NumberOfChannels);
    }

    private static float[] Normalize(Mat mat)
    {
        byte[] data = mat.GetRawData();
        float[] floatArray = new float[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            floatArray[i] = ((data[i] / 255.0f) - 0.5f) / 0.5f;
        }
        return floatArray;
    }

    private static float[] TransposeImage(float[] img, int height, int width, int channels)
    {
        float[] transposedImg = new float[img.Length];
        for (int c = 0; c < channels; c++)
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    int oldIndex = (h * width + w) * channels + c;
                    int newIndex = (c * height * width) + h * width + w;
                    transposedImg[newIndex] = img[oldIndex];
                }
            }
        }
        return transposedImg;
    }
}