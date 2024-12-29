using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.WindowParameters;

namespace MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;
public partial class PreviewFaceSwapTemplateViewModel(IFaceMultiSwapManager faceMultiSwapManager, ICameraManager cameraManager) :
    BaseFaceSwapTemplateViewModel(faceMultiSwapManager, cameraManager), IWindowParam<PreviewFaceSwapTemplateParameters>
{
    private PreviewFaceSwapTemplateParameters? _parameters;
    public PreviewFaceSwapTemplateParameters? Parameters
    {
        get => _parameters;
        set
        {
            _parameters = value;
            OnPropertyChanged(nameof(Parameters));
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
}
