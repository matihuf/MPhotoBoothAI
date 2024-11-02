using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Avalonia.Design.ViewModels
{
    public class DesignCameraSettingsViewModel : CameraSettingsViewModel
    {
        public DesignCameraSettingsViewModel() : base(new Mock<ICameraManager>().Object)
        {

        }
    }
}
