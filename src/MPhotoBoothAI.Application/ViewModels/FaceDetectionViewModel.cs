using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class FaceDetectionViewModel : ViewModelBase, IObserver, IDisposable
{
    private readonly IFaceDetectionService _yoloFaceService;
    private readonly ICameraService _cameraService;

    [ObservableProperty]
    private Mat _frame;

    public FaceDetectionViewModel(ICameraService cameraService, IFaceDetectionService yoloFaceService)
    {
        _cameraService = cameraService;
        _cameraService.Start();
        _cameraService.Attach(this);
        _yoloFaceService = yoloFaceService;
    }

    public void Notify(Mat mat)
    {
        var face = _yoloFaceService.Detect(mat, 0.45f, 0.5f).First();
        DrawPred(mat, face.Box, face.Confidence);
        Frame = mat.Clone();
        mat.Dispose();
    }

        private static void DrawPred(Mat frame, Rectangle box, float conf)
    {
        CvInvoke.Rectangle(frame, box, new MCvScalar(0, 0, 255), 3);
        CvInvoke.PutText(frame, conf.ToString(), new Point(100, 100), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 5, new MCvScalar(0, 0, 255), 1);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cameraService.Detach(this);
            Frame.Dispose();
        }
    }
}
