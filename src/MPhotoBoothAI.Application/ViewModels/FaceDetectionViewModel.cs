using Emgu.CV;

namespace MPhotoBoothAI.Application.ViewModels;

public class FaceDetectionViewModel(IYoloFaceService yoloFaceService) : ViewModelBase, IObserver
{
    private readonly IYoloFaceService _yoloFaceService = yoloFaceService;

    public Mat Frame { get; set; }
    public string Greeting => "Welcome to Avalonia!";

    public void Notify(Mat mat)
    {
        Frame = mat.Clone();
        _yoloFaceService.Run(mat, 0.45f, 0.5f);
        mat.Dispose();
    }
}
