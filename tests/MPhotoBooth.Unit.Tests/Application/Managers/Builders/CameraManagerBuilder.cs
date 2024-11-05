using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Managers;

namespace MPhotoBooth.Unit.Tests.Application.Managers.Builders
{
    public class CameraManagerBuilder
    {
        private readonly List<Mock<ICameraDevice>> _cameraDeviceMocks;
        private CameraManager _cameraManager;

        public CameraManagerBuilder()
        {
            _cameraDeviceMocks = new List<Mock<ICameraDevice>>();
        }

        public CameraManagerBuilder WithAvailableCamera()
        {
            var availableCameraMock = new Mock<ICameraDevice>();
            availableCameraMock.Setup(c => c.IsAvailable).Returns(true);
            _cameraDeviceMocks.Add(availableCameraMock);
            return this;
        }

        public CameraManagerBuilder WithUnavailableCamera()
        {
            var unavailableCameraMock = new Mock<ICameraDevice>();
            unavailableCameraMock.Setup(c => c.IsAvailable).Returns(false);
            _cameraDeviceMocks.Add(unavailableCameraMock);
            return this;
        }

        public CameraManagerBuilder WithCamera(Mock<ICameraDevice> cameraMock)
        {
            _cameraDeviceMocks.Add(cameraMock);
            return this;
        }

        public CameraManager Build()
        {
            var cameraDevices = _cameraDeviceMocks.ConvertAll(mock => mock.Object);
            _cameraManager = new CameraManager(cameraDevices);
            return _cameraManager;
        }

        public List<Mock<ICameraDevice>> GetCameraMocks()
        {
            return _cameraDeviceMocks;
        }
    }
}
