using CommunityToolkit.Mvvm.ComponentModel;

namespace MPhotoBoothAI.Models.Camera
{
    public partial class CurrentCameraSettings : ObservableObject
    {
        [ObservableProperty]
        private CameraSetting? _iso;

        [ObservableProperty]
        private CameraSetting? _aperture;

        [ObservableProperty]
        private CameraSetting? _shutterSpeed;

        [ObservableProperty]
        private CameraSetting? _whiteBalance;
    }
}
