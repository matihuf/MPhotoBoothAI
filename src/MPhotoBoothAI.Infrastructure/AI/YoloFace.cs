using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MPhotoBoothAI.Infrastructure.Models;

namespace MPhotoBoothAI.Infrastructure;

public class YoloFace(string modelPath, float confThreshold, float nmsThreshold)
{
    private readonly string _modelPath = modelPath;
    private readonly float _confThreshold = confThreshold;
    private readonly float _nmsThreshold = nmsThreshold;


    public void Detect(Mat frame)
    {

    }


}
