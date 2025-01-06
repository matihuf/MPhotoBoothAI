using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.WindowParameters;

namespace MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;
public partial class PreviewFaceSwapTemplateViewModel(IFaceMultiSwapManager faceMultiSwapManager, ICameraManager cameraManager, IFaceDetectionManager faceDetectionManager) :
    BaseFaceSwapTemplateViewModel(faceMultiSwapManager, cameraManager), IWindowParam<PreviewFaceSwapTemplateParameters>
{
    private readonly IFaceDetectionManager _faceDetectionManager = faceDetectionManager;

    [ObservableProperty]
    private Mat? _preview;

    private PreviewFaceSwapTemplateParameters? _parameters;
    public PreviewFaceSwapTemplateParameters? Parameters
    {
        get => _parameters;
        set
        {
            _parameters = value;
            if (_parameters != null)
            {
                ClearPreview();
                var frame = CvInvoke.Imread(_parameters.FilePath);
                _faceDetectionManager.Mark(frame, 0.8f, 0.5f);
                Preview = frame;
            }
            OnPropertyChanged(nameof(Parameters));
        }
    }

    protected void ClearPreview()
    {
        if (Preview != null)
        {
            Preview.Dispose();
            Preview = null;
        }
    }

    [RelayCommand]
    private Task Swap()
    {
        if (Parameters != null)
        {
            return Swap(Parameters.FilePath);
        }
        return Task.CompletedTask;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            ClearPreview();
        }
    }
}
