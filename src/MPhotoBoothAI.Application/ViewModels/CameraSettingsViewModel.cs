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

        private readonly ICameraSettingsService _cameraSettingsService;

        [ObservableProperty]
        private Mat _frame;

        [ObservableProperty]
        private ICameraDevice? _currentCameraDevice;

        [ObservableProperty]
        private IEnumerable<ICameraDevice> _availables;

        [ObservableProperty]
        private CurrentCameraSettings? _currentCameraSettings = new CurrentCameraSettings();

        public CameraSettingsViewModel(ICameraManager cameraManager, ICameraSettingsService cameraSettingsService)
        {
            _cameraManager = cameraManager;
            _cameraSettingsService = cameraSettingsService;
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
            var cameraSettings = _cameraManager.GetCurrentCameraSettings();
            if (cameraSettings is not null)
            {
                cameraSettings.Iso.Current = _cameraSettingsService.Value.Iso;
                cameraSettings.Aperture.Current = _cameraSettingsService.Value.Aperture;
                cameraSettings.ShutterSpeed.Current = _cameraSettingsService.Value.ShutterSpeed;
                cameraSettings.WhiteBalance.Current = _cameraSettingsService.Value.WhiteBalance;
                CurrentCameraSettings.Iso = cameraSettings.Iso;
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
        private void CameraSettingsChanged()
        {
            _cameraSettingsService.Value.Iso = CurrentCameraSettings.Iso?.Current ?? string.Empty;
            _cameraSettingsService.Value.Aperture = CurrentCameraSettings.Aperture?.Current ?? string.Empty;
            _cameraSettingsService.Value.ShutterSpeed = CurrentCameraSettings.ShutterSpeed?.Current ?? string.Empty;
            _cameraSettingsService.Value.WhiteBalance = CurrentCameraSettings.WhiteBalance?.Current ?? string.Empty;
            _cameraManager.SetCurrentCameraSettings(CurrentCameraSettings);
        }
    }
}