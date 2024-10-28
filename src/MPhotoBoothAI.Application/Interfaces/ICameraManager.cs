using MPhotoBoothAI.Models.Camera;

namespace MPhotoBoothAI.Application.Interfaces
{
    public interface ICameraManager : IDisposable
    {
        IEnumerable<ICameraDevice> Availables { get; }

        ICameraDevice? Current { get; set; }

        CurrentCameraSettings? GetCurrentCameraSettings();

        void SetCurrentCameraSettings(CurrentCameraSettings currentCameraSettings);

    }
}