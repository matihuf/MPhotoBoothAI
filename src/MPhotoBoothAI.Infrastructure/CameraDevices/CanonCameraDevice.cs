using EDSDK.NET;
using Emgu.CV;
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

        public event EventHandler Connected;

        public event EventHandler Disconnected;

        public bool IsAvailable { get; private set; } = false;

        public string CameraName { get; private set; }

        public CanonCameraDevice()
        {
            _diskInfoService = new DiskInfoService();
            _sdkHandler = new SDKHandler();
            Reconnect();
            _sdkHandler.ImageDownloaded += ImageDownloaded;
            _sdkHandler.CameraAdded += CameraAdded;
            _sdkHandler.CameraHasShutdown += CameraHasShutdown;
        }

        public CurrentCameraSettings? GetCurrentSettings()
        {
            return new CurrentCameraSettings
            {
                Iso = GetIso(),
                Aperture = GetAperture(),
                ShutterSpeed = GetShutterSpeed(),
                WhiteBalance = GetWhiteBalance()
            };
        }

        public void SetCurrentSettings(CurrentCameraSettings currentCameraSettings)
        {
            if (currentCameraSettings?.Iso?.Current != null)
            {
                SetIso(currentCameraSettings.Iso.Current);
            }
            if (currentCameraSettings?.Aperture?.Current != null)
            {
                SetAperture(currentCameraSettings.Aperture.Current);
            }
            if (currentCameraSettings?.ShutterSpeed?.Current != null)
            {
                SetShutterSpeed(currentCameraSettings.ShutterSpeed.Current);
            }
            if (currentCameraSettings?.WhiteBalance?.Current != null)
            {
                SetWhiteBalance(currentCameraSettings.WhiteBalance.Current);
            }
        }

        private CameraSetting GetAperture()
        {
            var apertureSettings = new List<string>();
            foreach (var aperture in _sdkHandler.GetSettingsList(PropID_Av))
            {
                if (CameraValues.AvValues.TryGetValue((uint)aperture, out string? dictApertureValue))
                {
                    apertureSettings.Add(dictApertureValue);
                }
            }
            return new CameraSetting
            {
                AvailableValues = apertureSettings,
                Current = CameraValues.AvValues.TryGetValue(_sdkHandler.GetSetting(PropID_Av), out string? apertureValue) ? apertureValue : string.Empty
            };
        }

        private void SetAperture(string? aperatureValue)
        {
            _sdkHandler.SetSetting(PropID_Av, CameraValues.AvValues.FirstOrDefault(v => v.Value == aperatureValue).Key);
        }

        private CameraSetting GetIso()
        {
            var isoSettings = new List<string>();
            foreach (var iso in _sdkHandler.GetSettingsList(PropID_ISOSpeed))
            {
                if (CameraValues.IsoValues.TryGetValue((uint)iso, out string? dictIsoValue))
                {
                    isoSettings.Add(dictIsoValue);
                }
            }
            return new CameraSetting
            {
                AvailableValues = isoSettings,
                Current = CameraValues.IsoValues.TryGetValue(_sdkHandler.GetSetting(PropID_ISOSpeed), out string? isoValue) ? isoValue : string.Empty
            };
        }

        private void SetIso(string isoValue)
        {
            _sdkHandler.SetSetting(PropID_ISOSpeed, CameraValues.IsoValues.FirstOrDefault(v => v.Value == isoValue).Key);
        }

        private CameraSetting GetShutterSpeed()
        {
            var shutterSpeed = new List<string>();
            foreach (var shutter in _sdkHandler.GetSettingsList(PropID_Tv))
            {
                if (CameraValues.TvValues.TryGetValue((uint)shutter, out string? dictShutterSpeed))
                {
                    shutterSpeed.Add(dictShutterSpeed);
                }
            }
            return new CameraSetting
            {
                AvailableValues = shutterSpeed,
                Current = CameraValues.TvValues.TryGetValue(_sdkHandler.GetSetting(PropID_Tv), out string? shutterSpeedValue) ? shutterSpeedValue : string.Empty
            };
        }

        private void SetShutterSpeed(string shutterSpeedValue)
        {
            _sdkHandler.SetSetting(PropID_Tv, CameraValues.TvValues.FirstOrDefault(v => v.Value == shutterSpeedValue).Key);
        }

        private CameraSetting GetWhiteBalance()
        {
            var whiteBalance = CameraValues.WhiteBalanceValues.Select(w => w.Value);
            return new CameraSetting
            {
                AvailableValues = whiteBalance,
                Current = CameraValues.WhiteBalanceValues.TryGetValue(_sdkHandler.GetSetting(PropID_WhiteBalance), out string? whiteBalanceValue) ? whiteBalanceValue : string.Empty
            };
        }

        private void SetWhiteBalance(string whiteBalanceValue)
        {
            _sdkHandler.SetSetting(PropID_WhiteBalance, CameraValues.WhiteBalanceValues.FirstOrDefault(v => v.Value == whiteBalanceValue).Key);
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
                _sdkHandler.SetSetting(PropID_AEModeSelect, AEMode_Manual);
            }
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}