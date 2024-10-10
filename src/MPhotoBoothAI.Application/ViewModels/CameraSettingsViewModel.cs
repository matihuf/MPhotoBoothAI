using CommunityToolkit.Mvvm.ComponentModel;
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
        private ICameraDevice _currentCameraDevice;

        [ObservableProperty]
        private List<ICameraDevice> _cameraDevices;

        public CameraSettingsViewModel(IEnumerable<ICameraDevice> cameraDevices)
        {
            _cameraDevices = new(cameraDevices);
        }

        public void Notify(Mat mat)
        {
            if (mat != null)
            {
                Frame = mat;
            }
        }

        partial void OnCurrentCameraDeviceChanged(ICameraDevice? oldValue, ICameraDevice newValue)
        {
            oldValue?.StopLiveView();
            oldValue?.Detach(this);
            newValue?.Attach(this);
            newValue?.Connect();
            newValue?.TakePhotoAsync();
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
                foreach (var cameraDevice in _cameraDevices)
                {
                    cameraDevice?.Detach(this);
                    cameraDevice?.Dispose();
                }
                Frame.Dispose();
            }
        }
    }
}