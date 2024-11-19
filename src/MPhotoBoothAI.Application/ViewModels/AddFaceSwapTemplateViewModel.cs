using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;
using MPhotoBoothAI.Models.FaceSwaps;
using MPhotoBoothAI.Models.WindowParameters;

namespace MPhotoBoothAI.Application.ViewModels;
public partial class AddFaceSwapTemplateViewModel : ViewModelBase, IObserver, IDisposable, IWindowParam<AddFaceSwapTemplateParameters>
{
    private readonly IAddFaceSwapTemplateManager _addFaceSwapTemplateManager;

    [ObservableProperty]
    private ICameraDevice? _cameraDevice;

    [ObservableProperty]
    private Mat? _image;

    [ObservableProperty]
    private Mat? _cameraFrame;

    [ObservableProperty]
    private bool _topmost = false;

    [ObservableProperty]
    private bool _isFaceDetectionProgressActive;

    [ObservableProperty]
    private FaceSwapTemplate? _faceSwapTemplate;

    [ObservableProperty]
    private bool _saveButtonIsEnabled;

    [ObservableProperty]
    private bool _swapButtonIsEnabled;

    public AddFaceSwapTemplateParameters? Parameters { get; set; }

    public AddFaceSwapTemplateViewModel(IAddFaceSwapTemplateManager addFaceSwapTemplateManager, ICameraManager cameraManager)
    {
        _addFaceSwapTemplateManager = addFaceSwapTemplateManager;
        _cameraDevice = cameraManager.Current;
        _cameraDevice?.Attach(this);
    }

    [RelayCommand]
    private async Task PickTemplate()
    {
        try
        {
            ClearImage();
            IsFaceDetectionProgressActive = true;
            FaceSwapTemplate = null;
            Topmost = false;
            FaceSwapTemplate = await _addFaceSwapTemplateManager.PickTemplate();
        }
        finally
        {
            IsFaceDetectionProgressActive = false;
            Topmost = true;
        }
    }

    [RelayCommand]
    private void Swap()
    {
        if (FaceSwapTemplate != null && CameraFrame != null)
        {
            using var templateImage = CvInvoke.Imread(FaceSwapTemplate.FilePath);
            using var cameraFrameTmp = CameraFrame.Clone();
            ClearImage();
            Image = _addFaceSwapTemplateManager.SwapFaces(cameraFrameTmp, templateImage);
        }
    }

    [RelayCommand]
    private static void Close(IMainWindow mainWindow) => mainWindow.Close();

    [RelayCommand]
    private void Save(IMainWindow mainWindow)
    {
        if (FaceSwapTemplate != null && Parameters != null)
        {
            _addFaceSwapTemplateManager.SaveTemplate(Parameters.GroupName, FaceSwapTemplate);
            Close(mainWindow);
        }
    }

    private void ClearImage()
    {
        if (Image != null)
        {
            Image.Dispose();
            Image = null;
        }
    }

    private void ClearCameraFrame()
    {
        if (CameraFrame != null)
        {
            CameraFrame.Dispose();
            CameraFrame = null;
        }
    }

    partial void OnFaceSwapTemplateChanged(FaceSwapTemplate? oldValue, FaceSwapTemplate? newValue)
    {
        SaveButtonIsEnabled = newValue != null && newValue.Faces > 0;
        SwapButtonIsEnabled = SaveButtonIsEnabled && CameraFrame != null;
    }

    public void Notify(Mat mat) => CameraFrame = mat;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            CameraDevice?.Detach(this);
            ClearImage();
            ClearCameraFrame();
            FaceSwapTemplate?.Dispose();
        }
    }
}
