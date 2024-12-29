using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.FaceSwaps;
using MPhotoBoothAI.Models.WindowParameters;
using MPhotoBoothAI.Models.WindowResults;

namespace MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;
public partial class AddFaceSwapTemplateViewModel(IAddFaceSwapTemplateManager addFaceSwapTemplateManager, IFaceMultiSwapManager faceMultiSwapManager, ICameraManager cameraManager) :
    BaseFaceSwapTemplateViewModel(faceMultiSwapManager, cameraManager), IWindowParam<AddFaceSwapTemplateParameters>, IWindowResult<AddFaceSwapTemplateResults>
{
    private readonly IAddFaceSwapTemplateManager _addFaceSwapTemplateManager = addFaceSwapTemplateManager;

    [ObservableProperty]
    private bool _isFaceDetectionProgressActive;

    [ObservableProperty]
    private FaceSwapTemplate? _faceSwapTemplate;

    [ObservableProperty]
    private bool _saveButtonIsEnabled;

    [ObservableProperty]
    private bool _swapButtonIsEnabled;

    public AddFaceSwapTemplateParameters? Parameters { get; set; }

    public AddFaceSwapTemplateResults? Result { get; set; }

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
    private Task Swap()
    {
        if (FaceSwapTemplate != null)
        {
            return Swap(FaceSwapTemplate.FilePath);
        }
        return Task.CompletedTask;
    }

    [RelayCommand]
    private void Save(IMainWindow mainWindow)
    {
        if (FaceSwapTemplate != null && Parameters != null)
        {
            var templateId = _addFaceSwapTemplateManager.SaveTemplate(Parameters.GroupId, FaceSwapTemplate);
            Result = new AddFaceSwapTemplateResults(templateId, FaceSwapTemplate.Faces);
            Close(mainWindow);
        }
    }

    partial void OnFaceSwapTemplateChanged(FaceSwapTemplate? oldValue, FaceSwapTemplate? newValue)
    {
        SaveButtonIsEnabled = newValue != null && newValue.Faces > 0;
        SwapButtonIsEnabled = SaveButtonIsEnabled && CameraFrame != null;
    }
}
