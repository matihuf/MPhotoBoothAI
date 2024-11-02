using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Application.Interfaces;

public interface ICameraDeviceSettings
{
    public string Iso { get; set; }
    public string Aperture { get; set; }
    public string ShutterSpeed { get; set; }
    public string WhiteBalance { get; set; }

    public ObservableCollection<string> IsoValues { get; set; }
    public ObservableCollection<string> ApertureValues { get; set; }
    public ObservableCollection<string> ShutterSpeedValues { get; set; }
    public ObservableCollection<string> WhiteBalanceValues { get; set; }

    public event EventHandler<string> CameraSettingChanged;
}