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
        private readonly Dictionary<string, Action> _cameraSettingByStringKey = [];

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
            //_cameraSettingByStringKey = new Dictionary<string, Action>()
            //{
            //    {"Iso", SetIso },
            //    {"ShutterSpeed", SetShutterSpeed },
            //    {"Aperture", SetAperture },
            //    {"WhiteBalance", SetWhiteBalance },
            //};
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

        partial void OnCurrentCameraSettingsChanged(CurrentCameraSettings? value)
        {
            if (value is not null)
            {
                _cameraSettingsService.Value.Iso = value.Iso.Current;
                _cameraSettingsService.Value.Aperture = value.Aperture.Current;
                _cameraSettingsService.Value.ShutterSpeed = value.ShutterSpeed.Current;
                _cameraSettingsService.Value.WhiteBalance = value.WhiteBalance.Current;
                _cameraManager.SetCurrentCameraSettings(value);
            }
        }

        private void LoadSettingsFromDatabase()
        {
            var cameraSettings = _cameraManager.GetCurrentCameraSettings();
            cameraSettings.Iso.Current = _cameraSettingsService.Value.Iso;
            cameraSettings.Aperture.Current = _cameraSettingsService.Value.Aperture;
            cameraSettings.ShutterSpeed.Current = _cameraSettingsService.Value.ShutterSpeed;
            cameraSettings.WhiteBalance.Current = _cameraSettingsService.Value.WhiteBalance;
            CurrentCameraSettings = cameraSettings;
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
    }
}