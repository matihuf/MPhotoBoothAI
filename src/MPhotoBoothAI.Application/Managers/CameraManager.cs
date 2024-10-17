using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.Camera;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MPhotoBoothAI.Infrastructure.Services
{
    public class CameraManager : ICameraManager, INotifyPropertyChanged
    {
        private readonly IEnumerable<ICameraDevice> _cameras;

        public CameraManager(IEnumerable<ICameraDevice> cameras)
        {
            _cameras = cameras;
            foreach (ICameraDevice camera in _cameras)
            {
                camera.Connected += Camera_AvilableChanged;
                camera.Disconnected += Camera_AvilableChanged;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public IEnumerable<ICameraDevice> Availables => _cameras != null ? _cameras.Where(x => x.IsAvailable) : [];

        public ICameraDevice? Current { get; set; }

        private ICameraDeviceSettings? CurrentSettings => Current as ICameraDeviceSettings;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public CameraSetting? GetAperture() => CurrentSettings?.GetAperture();

        public CameraSetting? GetIsoSettings() => CurrentSettings?.GetIsoSetting();

        public CameraSetting? GetProgram() => CurrentSettings?.GetProgram();

        public CameraSetting? GetShutterSpeed() => CurrentSettings?.GetShutterSpeed();

        public CameraSetting? GetWhiteBalance() => CurrentSettings?.GetWhiteBalance();

        public void SetAperture(string apertureValue) => CurrentSettings?.SetAperture(apertureValue);

        public void SetIso(string isoValue) => CurrentSettings?.SetIso(isoValue);


        public void SetProgram(string programValue) => CurrentSettings?.SetProgram(programValue);

        public void SetShutterSpeed(string shutterSpeedValue) => CurrentSettings?.SetShutterSpeed(shutterSpeedValue);

        public void SetWhiteBalance(string whiteBalanceValue) => CurrentSettings?.SetWhiteBalance(whiteBalanceValue);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (ICameraDevice camera in _cameras)
                {
                    camera.Connected -= Camera_AvilableChanged;
                    camera.Disconnected -= Camera_AvilableChanged;
                }
            }
        }

        private void Camera_AvilableChanged(object? sender, EventArgs e) => NotifyPropertyChanged(nameof(Availables));

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
