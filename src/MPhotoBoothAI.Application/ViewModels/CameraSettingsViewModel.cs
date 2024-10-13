using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.ViewModels
{
    public partial class CameraSettingsViewModel : ViewModelBase, IObserver, IDisposable
    {
        [ObservableProperty]
        private Mat _frame;

        [ObservableProperty]
        private ICameraDevice? _currentCameraDevice;

        [ObservableProperty]
        private ICameraManager _cameraManager;

        public CameraSettingsViewModel(ICameraManager cameraManager)
        {
            _cameraManager = cameraManager;
            CurrentCameraDevice = _cameraManager.Availables.FirstOrDefault();
        }

        public void Notify(Mat mat)
        {
            Frame = mat;
        }

        partial void OnCurrentCameraDeviceChanged(ICameraDevice? oldValue, ICameraDevice? newValue)
        {
            oldValue?.StopLiveView();
            oldValue?.Detach(this);
            newValue?.Attach(this);
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