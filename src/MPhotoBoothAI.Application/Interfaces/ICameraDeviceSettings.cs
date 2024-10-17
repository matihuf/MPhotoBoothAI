using MPhotoBoothAI.Models.Camera;

namespace MPhotoBoothAI.Application.Interfaces
{
    public interface ICameraDeviceSettings
    {
        CameraSetting GetIsoSetting();

        void SetIso(string isoValue);

        CameraSetting GetShutterSpeed();

        void SetShutterSpeed(string shutterSpeedValue);

        CameraSetting GetProgram();

        void SetProgram(string programValue);

        CameraSetting GetWhiteBalance();

        void SetWhiteBalance(string whiteBalanceValue);

        CameraSetting GetAperture();

        void SetAperture(string aperatureValue);
    }
}