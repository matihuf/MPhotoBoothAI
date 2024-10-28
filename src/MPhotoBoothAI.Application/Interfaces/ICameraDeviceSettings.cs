using MPhotoBoothAI.Models.Camera;

namespace MPhotoBoothAI.Application.Interfaces
{
    public interface ICameraDeviceSettings
    {
        CurrentCameraSettings? GetCurrentSettings();

        void SetCurrentSettings(CurrentCameraSettings currentCameraSettings);
    }
}