using MPhotoBoothAI.Models.Base;

namespace MPhotoBoothAI.Models
{
    public class CameraSettings : BaseSettings
    {
        private string _iso = string.Empty;

        public string Iso
        {
            get => _iso;
            set
            {
                _iso = value;
                NotifyPropertyChanged(value);
            }
        }

        private string _aperture = string.Empty;

        public string Aperture
        {
            get => _aperture;
            set
            {
                _aperture = value;
                NotifyPropertyChanged(value);
            }
        }

        private string _shutterSpeed = string.Empty;

        public string ShutterSpeed
        {
            get => _shutterSpeed;
            set
            {
                _shutterSpeed = value;
                NotifyPropertyChanged(value);
            }
        }

        private string _whiteBalance = string.Empty;

        public string WhiteBalance
        {
            get => _whiteBalance;
            set
            {
                _whiteBalance = value;
                NotifyPropertyChanged(value);
            }
        }
    }
}