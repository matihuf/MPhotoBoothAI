using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;
using MPhotoBoothAI.Models.Camera;

namespace MPhotoBoothAI.Application.ViewModels
{
    public partial class CameraSettingsViewModel : ViewModelBase, IObserver, IDisposable
    {
        private readonly ICameraManager _cameraManager;

        private readonly IDatabaseContext _databaseContext;

        [ObservableProperty]
        private Mat _frame;

        [ObservableProperty]
        private ICameraDevice? _currentCameraDevice;

        [ObservableProperty]
        private IEnumerable<ICameraDevice> _availables;

        [ObservableProperty]
        private CurrentCameraSettings? _currentCameraSettings;

        public CameraSettingsViewModel(ICameraManager cameraManager, IDatabaseContext databaseContext)
        {
            _cameraManager = cameraManager;
            _databaseContext = databaseContext;
            _availables = _cameraManager.Availables;
            Frame = new Mat();
        }

        public void Notify(Mat mat) => Frame = mat;

        partial void OnCurrentCameraDeviceChanged(ICameraDevice? oldValue, ICameraDevice? newValue)
        {
            oldValue?.StopLiveView();
            oldValue?.Detach(this);
            newValue?.Attach(this);
            _cameraManager.Current = newValue;
            LoadSettingsFromDatabase();
            GetCameraSettings();
        }

        private void LoadSettingsFromDatabase()
        {
            CurrentCameraSettings = _cameraManager?.GetCurrentCameraSettings();
            if (CurrentCameraSettings is not null)
            {
                var dataBaseCameraSettings = _databaseContext.CameraSettings.FirstOrDefault();
                if (dataBaseCameraSettings is not null)
                {
                    CurrentCameraSettings.Iso.Current = dataBaseCameraSettings.Iso;
                    CurrentCameraSettings.Aperture.Current = dataBaseCameraSettings.Aperture;
                    CurrentCameraSettings.ShutterSpeed.Current = dataBaseCameraSettings.ShutterSpeed;
                    CurrentCameraSettings.WhiteBalance.Current = dataBaseCameraSettings.WhiteBalance;
                }
            }
        }

        private void GetCameraSettings()
        {
            CurrentCameraSettings = _cameraManager.GetCurrentCameraSettings();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                CurrentCameraDevice?.Detach(this);
                Frame?.Dispose();
            }
        }

        [RelayCommand]
        private void TakePhoto() => CurrentCameraDevice?.TakePhoto();

        [RelayCommand]
        private void StartLiveView() => CurrentCameraDevice?.StartLiveView();

        [RelayCommand]
        private async Task CameraSettingsChanged()
        {
            var databaseCameraSettings = _databaseContext.CameraSettings.FirstOrDefault();
            if (databaseCameraSettings is not null && CurrentCameraSettings is not null)
            {
                databaseCameraSettings.Iso = CurrentCameraSettings?.Iso?.Current ?? string.Empty;
                databaseCameraSettings.Aperture = CurrentCameraSettings?.Aperture?.Current ?? string.Empty;
                databaseCameraSettings.ShutterSpeed = CurrentCameraSettings?.ShutterSpeed?.Current ?? string.Empty;
                databaseCameraSettings.WhiteBalance = CurrentCameraSettings?.WhiteBalance?.Current ?? string.Empty;
                await _databaseContext.SaveChangesAsync();
                _cameraManager.SetCurrentCameraSettings(CurrentCameraSettings);
            }
        }
    }
}