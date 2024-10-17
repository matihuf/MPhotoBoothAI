using EDSDK.NET;
using Emgu.CV;
using MPhotoBoothAI.Application.Enums;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Infrastructure.Services;
using MPhotoBoothAI.Models.Camera;
using static EDSDKLib.EDSDK;

namespace MPhotoBoothAI.Infrastructure.CameraDevices
{
    public class CanonCameraDevice : BaseCameraDevice, ICameraDevice, ICameraDeviceSettings
    {
        private readonly IDiskInfoService _diskInfoService;
        private readonly SDKHandler _sdkHandler;
        public CanonCameraDevice()
        {
            _diskInfoService = new DiskInfoService();
            _sdkHandler = new SDKHandler();
            Reconnect();
            _sdkHandler.ImageDownloaded += ImageDownloaded;
            _sdkHandler.CameraAdded += CameraAdded;
            _sdkHandler.CameraHasShutdown += CameraHasShutdown;
        }

        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public ECameraType CameraType => ECameraType.Canon;
        public bool IsAvailable { get; private set; } = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public CameraSetting GetAperture()
        {
            throw new NotImplementedException();
        }

        public CameraSetting GetIsoSetting()
        {
            var isoSettings = new List<string>();
            foreach (var iso in _sdkHandler.GetSettingsList(PropID_ISOSpeed))
            {
                isoSettings.Add(CameraValues.ISO(iso));
            }
            _ = GetProgram();
            return new CameraSetting
            {
                AvailableValues = isoSettings,
                Current = CameraValues.ISO((int)_sdkHandler.GetSetting(PropID_ISOSpeed))
            };
        }

        public CameraSetting GetProgram()
        {
            var programSettings = new List<string>();
            foreach (var program in _sdkHandler.GetSettingsList(PropID_AEMode))
            {
                //isoSettings.Add(CameraValues.ISO(iso));
            }
            return new CameraSetting
            {
                //AvailableValues = isoSettings,
                Current = CameraValues.ISO((int)_sdkHandler.GetSetting(PropID_ISOSpeed))
            };
        }

        public CameraSetting GetShutterSpeed()
        {
            return new CameraSetting();
            //throw new NotImplementedException();
        }

        public CameraSetting GetWhiteBalance()
        {
            return new CameraSetting();

            //throw new NotImplementedException();
        }

        public void SetAperture(string aperatureValue)
        {
            //throw new NotImplementedException();
        }

        public void SetIso(string isoValue) => _sdkHandler.SetSetting(PropID_ISOSpeed, CameraValues.ISO(isoValue));

        public void SetProgram(string programValue)
        {
            //throw new NotImplementedException();
        }

        public void SetShutterSpeed(string shutterSpeedValue)
        {
            //throw new NotImplementedException();
        }

        public void SetWhiteBalance(string whiteBalanceValue)
        {
            //throw new NotImplementedException();
        }

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

        private void CameraAdded()
        {
            Reconnect();
            Connected?.Invoke(this, EventArgs.Empty);
        }

        private void CameraHasShutdown(object? sender, EventArgs e)
        {
            IsAvailable = _sdkHandler.GetCameraList().Any();
            if (!IsAvailable)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }
        private void ImageDownloaded(Mat mat) => Notify(mat);

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
    }
}
