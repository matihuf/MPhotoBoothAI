using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.ViewModels
{
    public partial class CameraSettingsViewModel : ViewModelBase, IObserver, IDisposable
    {
        private readonly ICameraManager _cameraManager;

        [ObservableProperty]
        private Mat _frame;

        [ObservableProperty]
        private ICameraDevice? _currentCameraDevice;

        [ObservableProperty]
        private IEnumerable<ICameraDevice> _availables;

        [ObservableProperty]
        private ICameraDeviceSettings? _cameraSettings;

        public CameraSettingsViewModel(ICameraManager cameraManager)
        {
            _cameraManager = cameraManager;
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
            oldValue?.StopLiveView();
            oldValue?.Detach(this);
            newValue?.Attach(this);
            _cameraManager.Current = newValue;
            CameraSettings = newValue is ICameraDeviceSettings cameraDeviceSettings ? cameraDeviceSettings : null;
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
                CurrentCameraDevice?.Detach(this);
                Frame?.Dispose();
            }
        }
    }
}