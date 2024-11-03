using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;
using MPhotoBoothAI.Models.Entities;
using System.ComponentModel;

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
        private ICameraDeviceSettings? _cameraSettings;

        public CameraSettingsViewModel(ICameraManager cameraManager, IDatabaseContext databaseContext)
        {
            _cameraManager = cameraManager;
            _databaseContext = databaseContext;
            Availables = _cameraManager.Availables;
            Frame = new Mat();
            _cameraManager.OnAvaliableCameraListChanged += CameraListChanged;
        }

        private void CameraListChanged(IEnumerable<ICameraDevice> cameraList)
        {
            Availables = cameraList;
        }

        public void Notify(Mat mat) => Frame = mat;

        partial void OnCurrentCameraDeviceChanged(ICameraDevice? oldValue, ICameraDevice? newValue)
        {
            if (oldValue is ICameraDeviceSettings oldCameraSettings)
            {
                oldCameraSettings.PropertyChanged -= CameraSettings_CameraSettingChanged;
            }
            CameraSettings = newValue is ICameraDeviceSettings cameraDeviceSettings ? cameraDeviceSettings : null;
            if (CameraSettings != null)
            {
                CameraSettings.PropertyChanged += CameraSettings_CameraSettingChanged;
            }
            oldValue?.StopLiveView();
            oldValue?.Detach(this);
            newValue?.Attach(this);
            _cameraManager.Current = newValue;
            GetCameraSettingFromDatabase(newValue);
        }

        private void CameraSettings_CameraSettingChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Manual")
            {
                GetCameraSettingFromDatabase(CurrentCameraDevice);
                return;
            }
            if (CameraSettings != null && CurrentCameraDevice != null)
            {
                var cameraName = CurrentCameraDevice.CameraName;
                var cameraSettings = _databaseContext.CameraSettings.FirstOrDefault(s => s.Camera == cameraName);
                if (cameraSettings == null)
                {
                    cameraSettings = new CameraSettingsEntity
                    {
                        Camera = cameraName
                    };
                    _databaseContext.CameraSettings.Add(cameraSettings);
                }
                cameraSettings.Iso = CameraSettings.Iso;
                cameraSettings.Aperture = CameraSettings.Aperture;
                cameraSettings.ShutterSpeed = CameraSettings.ShutterSpeed;
                cameraSettings.WhiteBalance = CameraSettings.WhiteBalance;
                _databaseContext.SaveChanges();
            }
        }

        private void GetCameraSettingFromDatabase(ICameraDevice? newValue)
        {
            if (newValue != null && _databaseContext.CameraSettings.Any(s => s.Camera == newValue.CameraName) && newValue is ICameraDeviceSettings settings)
            {
                var dbCameraSettings = _databaseContext.CameraSettings.FirstOrDefault(s => s.Camera == newValue.CameraName);
                if (dbCameraSettings != null)
                {
                    settings.Iso = dbCameraSettings.Iso;
                    settings.Aperture = dbCameraSettings.Aperture;
                    settings.ShutterSpeed = dbCameraSettings.ShutterSpeed;
                    settings.WhiteBalance = dbCameraSettings.WhiteBalance;
                }
            }
        }

        [RelayCommand]
        private void TakePhoto() => CurrentCameraDevice?.TakePhoto();

        [RelayCommand]
        private void StartLiveView() => CurrentCameraDevice?.StartLiveView();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cameraManager.OnAvaliableCameraListChanged -= CameraListChanged;
                CurrentCameraDevice?.Detach(this);
                Frame?.Dispose();
            }
        }
    }
}