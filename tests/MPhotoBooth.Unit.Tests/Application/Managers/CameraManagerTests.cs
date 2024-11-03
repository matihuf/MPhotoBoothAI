using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Infrastructure.Services;

namespace MPhotoBooth.Unit.Tests.Application.Managers
{
    public class CameraManagerTests
    {
        [Fact]
        public void Availables_ShouldReturnOnlyAvailableCameras()
        {
            // Arrange
            var avaliableCamera = new Mock<ICameraDevice>();
            avaliableCamera.Setup(c => c.IsAvailable).Returns(true);

            var unAvaliableCamera = new Mock<ICameraDevice>();
            unAvaliableCamera.Setup(c => c.IsAvailable).Returns(false);

            var cameraList = new List<ICameraDevice> { avaliableCamera.Object, unAvaliableCamera.Object };
            var cameraManager = new CameraManager(cameraList);

            // Act
            var availableCameras = cameraManager.Availables;

            // Assert
            Assert.Single(availableCameras);
            Assert.Contains(avaliableCamera.Object, availableCameras);
        }

        [Fact]
        public void ChangingCameraAvailability_ShouldTriggerEvent()
        {
            // Arrange
            var camera = new Mock<ICameraDevice>();
            camera.Setup(c => c.IsAvailable).Returns(true);

            var cameraList = new List<ICameraDevice> { camera.Object };
            var cameraManager = new CameraManager(cameraList);

            bool eventTriggered = false;
            cameraManager.OnAvaliableCameraListChanged += (cameras) => eventTriggered = true;

            // Act
            camera.Raise(c => c.Connected += null, EventArgs.Empty);

            // Assert
            Assert.True(eventTriggered);
        }

        [Fact]
        public void Dispose_ShouldUnsubscribeFromCameraEvents()
        {
            // Arrange
            var camera = new Mock<ICameraDevice>();

            var cameraList = new List<ICameraDevice> { camera.Object };
            var cameraManager = new CameraManager(cameraList);

            // Act
            cameraManager.Dispose();

            // Assert
            camera.VerifyRemove(c => c.Connected -= It.IsAny<EventHandler>(), Times.Once);
            camera.VerifyRemove(c => c.Disconnected -= It.IsAny<EventHandler>(), Times.Once);
        }

        [Fact]
        public void Constructor_ShouldSubscribeToCameraEvents()
        {
            // Arrange
            var camera = new Mock<ICameraDevice>();

            var cameraList = new List<ICameraDevice> { camera.Object };

            // Act
            var cameraManager = new CameraManager(cameraList);

            // Assert
            camera.VerifyAdd(c => c.Connected += It.IsAny<EventHandler>(), Times.Once);
            camera.VerifyAdd(c => c.Disconnected += It.IsAny<EventHandler>(), Times.Once);
        }
    }
}
