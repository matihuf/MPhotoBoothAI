using EDSDK.NET;
using MPhotoBoothAI.Application.Enums;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.CameraDevices
{
    public class CanonCameraDevice : BaseCameraDevice, ICameraDevice
    {
        private Camera? _camera;
        private readonly SDKHandler _sdkHandler;
        private readonly IDiskInfoService _diskInfoService;

        public ECameraType CameraType => ECameraType.Canon;

        public CanonCameraDevice(IDiskInfoService diskInfoService, SDKHandler sdkHandler)
        {
            _sdkHandler = sdkHandler;
            _diskInfoService = diskInfoService;
        }

        private void SetDiskCapacity()
        {
            var (bytesPerSector, numberOfFreeClusters) = _diskInfoService.GetBytesPerSector();
            if (bytesPerSector is not null && numberOfFreeClusters is not null)
            {
                _sdkHandler.SetCapacity(bytesPerSector.Value, numberOfFreeClusters.Value);
            }
        }

        public void Dispose()
        {
            _sdkHandler?.Dispose();
        }

        public void StartLiveView()
        {
            _sdkHandler?.StartLiveView();
        }

        public void StopLiveView()
        {
            _sdkHandler?.StopLiveView();
        }

        public Task TakePhotoAsync(bool autoFocus = false)
        {
            _sdkHandler.TakePhoto();
            return Task.CompletedTask;
        }

        public bool Connect()
        {
            if (_sdkHandler.CameraSessionOpen)
            {
                _sdkHandler.CloseSession();
            }
            _camera = _sdkHandler.GetCameraList().FirstOrDefault();
            if (_camera is not null)
            {
                _sdkHandler.OpenSession(_camera);
                SetDiskCapacity();
                return true;
            }
            return false;
        }
    }
}
