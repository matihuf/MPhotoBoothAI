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

        private CameraSetting _iso = new();

        private CameraSetting _aperture = new();

        private CameraSetting _shutterSpeed = new();

        private CameraSetting _whiteBalance = new();

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

        public CurrentCameraSettings GetCurrentSettings()
        {
            _iso = GetIso();
            _aperture = GetAperture();
            _shutterSpeed = GetShutterSpeed();
            _whiteBalance = GetWhiteBalance();
            return new CurrentCameraSettings
            {
                Iso = _iso,
                Aperture = _aperture,
                ShutterSpeed = _shutterSpeed,
                WhiteBalance = _whiteBalance
            };
        }

        public void SetCurrentSettings(CurrentCameraSettings currentCameraSettings)
        {
            if (currentCameraSettings?.Iso?.Current != null)
            {
                SetIso(currentCameraSettings.Iso.Current);
                _iso.Current = currentCameraSettings.Iso.Current;
            }
            if (currentCameraSettings?.Aperture?.Current != null)
            {
                SetAperture(currentCameraSettings.Aperture.Current);
                _aperture.Current = currentCameraSettings.Aperture.Current;
            }
            if (currentCameraSettings?.ShutterSpeed?.Current != null)
            {
                SetShutterSpeed(currentCameraSettings.ShutterSpeed.Current);
                _shutterSpeed.Current = currentCameraSettings.ShutterSpeed.Current;
            }
            if (currentCameraSettings?.WhiteBalance?.Current != null)
            {
                SetWhiteBalance(currentCameraSettings.WhiteBalance.Current);
                _whiteBalance.Current = currentCameraSettings.WhiteBalance.Current;
            }
        }

        private CameraSetting GetAperture()
        {
            _aperture.Current = CameraValues.AvValues.TryGetValue(_sdkHandler.GetSetting(PropID_Av), out string? apertureValue) ? apertureValue : string.Empty;
            return _aperture;
        }

        private void SetAperture(string? aperatureValue)
        {
            _sdkHandler.SetSetting(PropID_Av, CameraValues.AvValues.FirstOrDefault(v => v.Value == aperatureValue).Key);
        }

        private CameraSetting GetIso()
        {
            _iso.Current = CameraValues.IsoValues.TryGetValue(_sdkHandler.GetSetting(PropID_ISOSpeed), out string? isoValue) ? isoValue : string.Empty;
            return _iso;
        }

        private void SetIso(string isoValue)
        {
            _sdkHandler.SetSetting(PropID_ISOSpeed, CameraValues.IsoValues.FirstOrDefault(v => v.Value == isoValue).Key);
        }

        private CameraSetting GetShutterSpeed()
        {
            _shutterSpeed.Current = CameraValues.TvValues.TryGetValue(_sdkHandler.GetSetting(PropID_Tv), out string? shutterSpeedValue) ? shutterSpeedValue : string.Empty;
            return _shutterSpeed;
        }

        private List<string> GetCanonPropValues(uint propID, Dictionary<uint, string> dictValues)
        {
            var listOfValues = new List<string>();
            foreach (var value in _sdkHandler.GetSettingsList(propID))
            {
                if (dictValues.TryGetValue((uint)value, out string? dictValue))
                {
                    listOfValues.Add(dictValue);
                }
            }
            return listOfValues;
        }

        private void SetShutterSpeed(string shutterSpeedValue)
        {
            _sdkHandler.SetSetting(PropID_Tv, CameraValues.TvValues.FirstOrDefault(v => v.Value == shutterSpeedValue).Key);
        }

        private CameraSetting GetWhiteBalance()
        {
            _whiteBalance.Current = CameraValues.WhiteBalanceValues.TryGetValue(_sdkHandler.GetSetting(PropID_WhiteBalance), out string? whiteBalanceValue) ? whiteBalanceValue : string.Empty;
            return _whiteBalance;
        }

        private void SetWhiteBalance(string whiteBalanceValue)
        {
            _sdkHandler.SetSetting(PropID_WhiteBalance, CameraValues.WhiteBalanceValues.FirstOrDefault(v => v.Value == whiteBalanceValue).Key);
        }

        private void SetCameraAvaliableValues()
        {
            _iso.AvailableValues = GetCanonPropValues(PropID_ISOSpeed, CameraValues.IsoValues);
            _aperture.AvailableValues = GetCanonPropValues(PropID_Av, CameraValues.AvValues);
            _shutterSpeed.AvailableValues = GetCanonPropValues(PropID_Tv, CameraValues.TvValues);
            _whiteBalance.AvailableValues = CameraValues.WhiteBalanceValues.Select(w => w.Value);
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
                SetCameraAvaliableValues();
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