using System.Drawing;
using Emgu.CV;
using Emgu.CV.Dnn;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Infrastructure.Services;

public class FaceGenderService([FromKeyedServices(Consts.AiModels.VggGender)] Net vggNet) : IFaceGenderService
{
    private readonly Net _vggNet = vggNet;
    private readonly Gender[] _genderList = [Gender.Female, Gender.Male];

    public Gender Get(Mat face)
    {
        using var input = DnnInvoke.BlobFromImage(face, 1.0, new Size(224, 224));
        _vggNet.SetInput(input);
        using Mat genders = _vggNet.Forward();
        double min = 0, max = 0;
        Point minP = Point.Empty, maxP = Point.Empty;
        CvInvoke.MinMaxLoc(genders, ref min, ref max, ref minP, ref maxP);
        return _genderList[maxP.X];
    }
}
