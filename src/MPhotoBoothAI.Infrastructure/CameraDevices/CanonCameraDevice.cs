using EDSDK.NET;
using Emgu.CV;
using MPhotoBoothAI.Application.Enums;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Infrastructure.Services;
using static EDSDKLib.EDSDK;

namespace MPhotoBoothAI.Infrastructure.CameraDevices
{
    public class CanonCameraDevice : BaseCameraDevice, ICameraDevice
    {
        private readonly SDKHandler _sdkHandler;
        private readonly IDiskInfoService _diskInfoService;

        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public ECameraType CameraType => ECameraType.Canon;
        public bool IsAvailable { get; private set; } = false;

        public CanonCameraDevice()
        {
            _diskInfoService = new DiskInfoService();
            _sdkHandler = new SDKHandler();
            Reconnect();
            _sdkHandler.ImageDownloaded += ImageDownloaded;
            _sdkHandler.CameraAdded += CameraAdded;
            _sdkHandler.CameraHasShutdown += CameraHasShutdown;
        }

        private void CameraHasShutdown(object? sender, EventArgs e)
        {
            IsAvailable = _sdkHandler.GetCameraList().Any();
            if (!IsAvailable)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CameraAdded()
        {
            Reconnect();
            Connected?.Invoke(this, EventArgs.Empty);
        }

        private void Reconnect()
        {
            var camera = _sdkHandler.GetCameraList().FirstOrDefault();
            if (camera != null)
            {
                IsAvailable = true;
                _sdkHandler.CloseSession();
                _sdkHandler.OpenSession(camera);
                var (bytesPerSector, numberOfFreeClusters) = _diskInfoService.GetBytesPerSector();
                if (bytesPerSector.HasValue && numberOfFreeClusters.HasValue)
                {
                    _sdkHandler.SetCapacity(bytesPerSector.Value, numberOfFreeClusters.Value);
                }
                _sdkHandler.SetSetting(PropID_SaveTo, (uint)EdsSaveTo.Host);
            }
        }

        private void ImageDownloaded(Mat mat) => Notify(mat);

        public void StartLiveView() => _sdkHandler?.StartLiveView();

        public void StopLiveView() => _sdkHandler?.StopLiveView();

        public void TakePhoto(bool autoFocus = false)
        {
            if (_sdkHandler.IsLiveViewOn)
            {
                _sdkHandler.StopLiveView();
            }
            _sdkHandler.TakePhoto();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _sdkHandler != null)
            {

                _sdkHandler.CloseSession();
                _sdkHandler.ImageDownloaded -= ImageDownloaded;
                _sdkHandler.CameraAdded -= CameraAdded;
                _sdkHandler.CameraHasShutdown -= CameraHasShutdown;
                _sdkHandler.Dispose();
            }
        }
    }
}
