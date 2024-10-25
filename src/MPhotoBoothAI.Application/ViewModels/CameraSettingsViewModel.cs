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
        private readonly Dictionary<string, Action> _setSettingByString = [];

        private readonly ICameraManager _cameraManager;

        [ObservableProperty]
        private Mat _frame;

        [ObservableProperty]
        private ICameraDevice? _currentCameraDevice;

        [ObservableProperty]
        private IEnumerable<ICameraDevice> _availables;

        [ObservableProperty]
        private CameraSetting? _isoSettings = new();

        [ObservableProperty]
        private CameraSetting? _shutterSettings = new();

        [ObservableProperty]
        private CameraSetting? _apertureSettings = new();

        [ObservableProperty]
        private CameraSetting? _whiteBalanceSettings = new();

        public CameraSettingsViewModel(ICameraManager cameraManager)
        {
            _cameraManager = cameraManager;
            _availables = _cameraManager.Availables;
            Frame = new Mat();
            _setSettingByString = new Dictionary<string, Action>()
            {
                {"Iso", SetIso },
                {"ShutterSpeed", SetShutterSpeed },
                {"Aperture", SetAperture },
                {"WhiteBalance", SetWhiteBalance },
            };
        }

        public void Notify(Mat mat) => Frame = mat;

        partial void OnCurrentCameraDeviceChanged(ICameraDevice? oldValue, ICameraDevice? newValue)
        {
            oldValue?.StopLiveView();
            oldValue?.Detach(this);
            newValue?.Attach(this);
            _cameraManager.Current = newValue;
            GetCameraSettings();
        }

        private void GetCameraSettings()
        {
            IsoSettings = _cameraManager.GetIso();
            ApertureSettings = _cameraManager.GetAperture();
            ShutterSettings = _cameraManager.GetShutterSpeed();
            WhiteBalanceSettings = _cameraManager.GetWhiteBalance();
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
        private void CameraSettingsChanged(string parameter)
        {
            if (_setSettingByString.ContainsKey(parameter))
            {
                _setSettingByString[parameter].Invoke();
            }
        }

        private void SetWhiteBalance()
        {
            if (!string.IsNullOrEmpty(WhiteBalanceSettings?.Current))
            {
                _cameraManager.SetWhiteBalance(WhiteBalanceSettings.Current);
            }
        }

        private void SetAperture()
        {
            if (!string.IsNullOrEmpty(ApertureSettings?.Current))
            {
                _cameraManager.SetAperture(ApertureSettings.Current);
            }
        }

        private void SetShutterSpeed()
        {
            if (!string.IsNullOrEmpty(ShutterSettings?.Current))
            {
                _cameraManager.SetShutterSpeed(ShutterSettings.Current);
            }
        }

        private void SetIso()
        {
            if (!string.IsNullOrEmpty(IsoSettings?.Current))
            {
                _cameraManager.SetIso(IsoSettings.Current);
            }
        }
    }
}