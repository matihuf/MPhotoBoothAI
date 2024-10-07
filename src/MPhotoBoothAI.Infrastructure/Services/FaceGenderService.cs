using System.Drawing;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Infrastructure.Services;

public class FaceGenderService([FromKeyedServices(Consts.AiModels.GoogleGender)] Net googleNet, [FromKeyedServices(Consts.AiModels.GoogleGender)] Net vggNet) : IFaceGenderService
{
    private readonly Net _googleNet = googleNet;
    private readonly Net _vggNet = vggNet;
    private readonly Gender[] _genderList = [Gender.Male, Gender.Female];

    public Gender Get(Mat face)
    {
        using var input = DnnInvoke.BlobFromImage(face, 1.0, new Size(224, 224), new MCvScalar(104, 117, 123));
        _googleNet.SetInput(input);
        using Mat genders = _googleNet.Forward();
        double min = 0, max = 0;
        Point minP = Point.Empty, maxP = Point.Empty;
        CvInvoke.MinMaxLoc(genders, ref min, ref max, ref minP, ref maxP);
        return _genderList[maxP.X];
    }

    public Gender GetVgg(Mat face)
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
