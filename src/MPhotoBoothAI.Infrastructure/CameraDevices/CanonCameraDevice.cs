using EDSDK.NET;
using Emgu.CV;
using Microsoft.Extensions.Logging;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Infrastructure.Services;
using System.Collections.ObjectModel;
using static EDSDKLib.EDSDK;

namespace MPhotoBoothAI.Infrastructure.CameraDevices
{
    public class CanonCameraDevice : BaseCameraDevice, ICameraDevice, ICameraDeviceSettings
    {
        private readonly IDiskInfoService _diskInfoService;

        private readonly SDKHandler _sdkHandler;

        private readonly ILogger<CanonCameraDevice> _logger;

        public event EventHandler? Connected;

        public event EventHandler? Disconnected;

        public event EventHandler<string> CameraSettingChanged;

        public bool IsAvailable { get; private set; } = false;

        public string Iso
        {
            get
            {
                if (_sdkHandler.CameraSessionOpen)
                {
                    return _sdkHandler.GetSetting(PropID_ISOSpeed, CameraValues.IsoValues);
                }
                return string.Empty;
            }
            set
            {
                if (value != null && _sdkHandler.CameraSessionOpen)
                {
                    _sdkHandler.SetSetting(PropID_ISOSpeed, value, CameraValues.IsoValues);
                    CameraSettingChanged?.Invoke(this, value);
                }
            }
        }

        public string Aperture
        {
            get
            {
                if (_sdkHandler.CameraSessionOpen)
                {
                    return _sdkHandler.GetSetting(PropID_Av, CameraValues.AvValues);
                }
                return string.Empty;
            }
            set
            {
                if (value != null && _sdkHandler.CameraSessionOpen)
                {
                    _sdkHandler.SetSetting(PropID_Av, value, CameraValues.AvValues);
                    CameraSettingChanged?.Invoke(this, value);
                }
            }
        }
        public string ShutterSpeed
        {
            get
            {
                if (_sdkHandler.CameraSessionOpen)
                {
                    return _sdkHandler.GetSetting(PropID_Tv, CameraValues.TvValues);
                }
                return string.Empty;
            }
            set
            {
                if (value != null && _sdkHandler.CameraSessionOpen)
                {
                    _sdkHandler.SetSetting(PropID_Tv, value, CameraValues.TvValues);
                    CameraSettingChanged?.Invoke(this, value);
                }
            }
        }
        public string WhiteBalance
        {
            get
            {
                if (_sdkHandler.CameraSessionOpen)
                {
                    return _sdkHandler.GetSetting(PropID_WhiteBalance, CameraValues.WhiteBalanceValues);
                }
                return string.Empty;
            }
            set
            {
                if (value != null && _sdkHandler.CameraSessionOpen)
                {
                    _sdkHandler.SetSetting(PropID_WhiteBalance, value, CameraValues.WhiteBalanceValues);
                    CameraSettingChanged?.Invoke(this, value);
                }
            }
        }

        public ObservableCollection<string> IsoValues { get; set; }

        public ObservableCollection<string> ApertureValues { get; set; }

        public ObservableCollection<string> ShutterSpeedValues { get; set; }

        public ObservableCollection<string> WhiteBalanceValues { get; set; }

        public string? CameraName { get; private set; }

        public CanonCameraDevice(ILogger<CanonCameraDevice> logger)
        {
            _diskInfoService = new DiskInfoService();
            _sdkHandler = new SDKHandler();
            _sdkHandler.ImageDownloaded += ImageDownloaded;
            _sdkHandler.CameraAdded += CameraAdded;
            _sdkHandler.CameraHasShutdown += CameraHasShutdown;
            _sdkHandler.SDKPropertyChangedEvent += CameraPropertyChanged;
            _sdkHandler.SDKErrorEvent += _sdkHandler_SdkErrorEvent;
            _logger = logger;
            Reconnect();
        }

        private void _sdkHandler_SdkErrorEvent(object? sender, uint e)
        {
            _logger.Log(LogLevel.Information, $"Canon SDK error: {Enum.GetName(typeof(CameraErrorCodes), (long)e)}");
        }

        private ObservableCollection<string> GetSetting(Dictionary<uint, string> cameraValues, uint propID) => new(GetCanonPropValues(propID, cameraValues));

        private IEnumerable<string> GetCanonPropValues(uint propID, Dictionary<uint, string> dictValues)
        {
            if (propID == PropID_WhiteBalance)
            {
                foreach (var item in CameraValues.WhiteBalanceValues.Select(p => p.Value))
                {
                    yield return item;
                }
            }
            else
            {
                foreach (var value in _sdkHandler.GetSettingsList(propID))
                {
                    if (dictValues.TryGetValue((uint)value, out string? dictValue))
                    {
                        yield return dictValue;
                    }
                }
            }
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
                _sdkHandler.UILock(true);
                _sdkHandler.SetSetting(PropID_AEModeSelect, AEMode_Manual);
                var (bytesPerSector, numberOfFreeClusters) = _diskInfoService.GetBytesPerSector();
                if (bytesPerSector.HasValue && numberOfFreeClusters.HasValue)
                {
                    _sdkHandler.SetCapacity(bytesPerSector.Value, numberOfFreeClusters.Value);
                }
                _sdkHandler.SetSetting(PropID_SaveTo, (uint)EdsSaveTo.Host);
                ApertureValues = GetSetting(CameraValues.AvValues, PropID_Av);
                IsoValues = GetSetting(CameraValues.IsoValues, PropID_ISOSpeed);
                ShutterSpeedValues = GetSetting(CameraValues.TvValues, PropID_Tv);
                WhiteBalanceValues = GetSetting(CameraValues.WhiteBalanceValues, PropID_WhiteBalance);
                CameraName = _sdkHandler.GetStringSetting(PropID_ProductName);
            }
        }

        private void CameraPropertyChanged(object? sender, uint e)
        {
            switch (e)
            {
                case PropID_ISOSpeed:
                    IsoValues = GetSetting(CameraValues.IsoValues, PropID_ISOSpeed);
                    break;
                case PropID_WhiteBalance:
                    WhiteBalanceValues = GetSetting(CameraValues.WhiteBalanceValues, PropID_WhiteBalance);
                    break;
                case PropID_Av:
                    ApertureValues = GetSetting(CameraValues.AvValues, PropID_Av);
                    break;
                case PropID_Tv:
                    ShutterSpeedValues = GetSetting(CameraValues.TvValues, PropID_Tv);
                    break;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _sdkHandler != null)
            {
                _sdkHandler.UILock(false);
                _sdkHandler.CloseSession();
                _sdkHandler.ImageDownloaded -= ImageDownloaded;
                _sdkHandler.CameraAdded -= CameraAdded;
                _sdkHandler.CameraHasShutdown -= CameraHasShutdown;
                _sdkHandler.SDKPropertyChangedEvent -= CameraPropertyChanged;
                _sdkHandler.SDKErrorEvent -= _sdkHandler_SdkErrorEvent;
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