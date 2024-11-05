using Moq;
using MPhotoBooth.Unit.Tests.Application.Managers.Builders;

namespace MPhotoBooth.Unit.Tests.Application.Managers
{
    public class CameraManagerTests
    {
        [Fact]
        public void Availables_ShouldReturnOnlyAvailableCameras()
        {
            // Arrange
            var builder = new CameraManagerBuilder()
                .WithAvailableCamera()
                .WithUnavailableCamera();
            var cameraManager = builder.Build();

            // Act
            var availableCameras = cameraManager.Availables;

            // Assert
            Assert.Single(availableCameras);
            var availableCameraMock = builder.GetCameraMocks().Find(m => m.Object.IsAvailable);
            Assert.Contains(availableCameraMock.Object, availableCameras);
        }

        [Fact]
        public void ChangingCameraAvailability_ShouldTriggerEvent()
        {
            // Arrange
            var builder = new CameraManagerBuilder().WithAvailableCamera();
            var cameraManager = builder.Build();

            bool eventTriggered = false;
            cameraManager.OnAvaliableCameraListChanged += (cameras) => eventTriggered = true;

            // Act
            var cameraMock = builder.GetCameraMocks()[0];
            cameraMock.Raise(c => c.Connected += null, EventArgs.Empty);

            // Assert
            Assert.True(eventTriggered);
        }

        [Fact]
        public void Dispose_ShouldUnsubscribeFromCameraEvents()
        {
            // Arrange
            var builder = new CameraManagerBuilder().WithAvailableCamera();
            var cameraManager = builder.Build();

            // Act
            cameraManager.Dispose();

            // Assert
            var cameraMock = builder.GetCameraMocks()[0];
            cameraMock.VerifyRemove(c => c.Connected -= It.IsAny<EventHandler>(), Times.Once);
            cameraMock.VerifyRemove(c => c.Disconnected -= It.IsAny<EventHandler>(), Times.Once);
        }

        [Fact]
        public void Constructor_ShouldSubscribeToCameraEvents()
        {
            // Arrange
            var builder = new CameraManagerBuilder().WithAvailableCamera();

            // Act
            var cameraManager = builder.Build();

            // Assert
            var cameraMock = builder.GetCameraMocks()[0];
            cameraMock.VerifyAdd(c => c.Connected += It.IsAny<EventHandler>(), Times.Once);
            cameraMock.VerifyAdd(c => c.Disconnected += It.IsAny<EventHandler>(), Times.Once);
        }
    }
}
