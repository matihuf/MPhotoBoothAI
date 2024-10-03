using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Runtime.InteropServices;

namespace MPhotoBoothAI.Integration.Tests;

public static class RawMatFile
{
    public static void MatToBase64File(Mat mat, string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        using var outputFile = new StreamWriter(path);
        outputFile.WriteLine(mat.Height);
        outputFile.WriteLine(mat.Width);
        outputFile.WriteLine((int)mat.Depth);
        outputFile.WriteLine(mat.NumberOfChannels);
        outputFile.WriteLine(mat.Step);
        outputFile.Write(Convert.ToBase64String(mat.GetRawData()));
    }

    public static Mat MatFromBase64File(string path)
    {
        using var inputFile = new StreamReader(path);
        int height = Convert.ToInt32(inputFile.ReadLine());
        int width = Convert.ToInt32(inputFile.ReadLine());
        DepthType depthType = (DepthType)Convert.ToInt32(inputFile.ReadLine());
        int numberOfChannels = Convert.ToInt32(inputFile.ReadLine());
        int step = Convert.ToInt32(inputFile.ReadLine());
        byte[] raw = Convert.FromBase64String(inputFile.ReadToEnd());
        GCHandle handle = GCHandle.Alloc(raw, GCHandleType.Pinned);
        using var image = new Mat(height, width, depthType, numberOfChannels, handle.AddrOfPinnedObject(), step);
        handle.Free();
        return image.Clone();
    }

    public static bool RawEqual(Mat source, Mat target, int margin = 2, float maxFailedPercentage = 1f)
    {
        var sourceRaw = source.GetRawData();
        var resultRaw = target.GetRawData();
        int failed = 0;
        for (int i = 0; i < sourceRaw.Length; i++)
        {
            if (Math.Abs(sourceRaw[i] - resultRaw[i]) > margin)
            {
                failed++;
            }
        }
        if (failed == 0)
        {
            return true;
        }
        var failedPercentage = (failed / (float)sourceRaw.Length) * 100f;
        return failedPercentage <= maxFailedPercentage;
    }
}
