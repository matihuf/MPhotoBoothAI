using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels.Bases;

namespace MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;
public partial class BaseFaceSwapTemplateViewModel(IFaceMultiSwapManager faceMultiSwapManager, ICameraManager cameraManager) : CameraBaseViewModel(cameraManager)
{
    private readonly IFaceMultiSwapManager _faceMultiSwapManager = faceMultiSwapManager;

    [ObservableProperty]
    private Mat? _image;

    private bool _topmost = true;
    public bool Topmost
    {
        get => !IsDebug && _topmost;
        set
        {
            _topmost = value;
            OnPropertyChanged(nameof(Topmost));
        }
    }

    [ObservableProperty]
    private bool _isProgressActive;

    [ObservableProperty]
    private bool _isEnabled = true;

    [ObservableProperty]
    private double _templateImageOpacity = 1;

    protected async Task Swap(string templateFilePath)
    {
        if (string.IsNullOrEmpty(templateFilePath) || CameraFrame == null)
        {
            return;
        }
        try
        {
            ClearImage();
            IsEnabled = false;
            TemplateImageOpacity = 0.3;
            IsProgressActive = true;
            var image = await Task.Run(() =>
            {
                using var template = CvInvoke.Imread(templateFilePath);
                using var cameraFrameTmp = CameraFrame.Clone();
                return _faceMultiSwapManager.Swap(cameraFrameTmp, template);
            });
            Image = image;
        }
        finally
        {
            IsEnabled = true;
            IsProgressActive = false;
            TemplateImageOpacity = 1;
        }
    }

    [RelayCommand]
    protected static void Close(IWindow mainWindow) => mainWindow.Close();

    protected void ClearImage()
    {
        if (Image != null)
        {
            Image.Dispose();
            Image = null;
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            ClearImage();
        }
    }
}
