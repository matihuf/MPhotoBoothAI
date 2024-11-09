using Emgu.CV;
using Moq;
using MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.Entities;
using System.ComponentModel;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels
{
    public class CameraSettingsViewModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            var builder = new CameraSettingsViewModelBuilder();

            var viewModel = builder.Build();
            // Assert
            Assert.NotNull(viewModel.Frame);
            Assert.Equal(builder.MockCameraManager.Object.Availables, viewModel.Availables);
        }

        [Fact]
        public void CurrentCameraDeviceChanged_ShouldUpdateCameraSettings()
        {
            var oldCameraDeviceMock = new Mock<ICameraDevice>();
            var newCameraDeviceMock = new Mock<ICameraDevice>();
            var builder = new CameraSettingsViewModelBuilder().WithMockedICamera(oldCameraDeviceMock, newCameraDeviceMock);
            var viewModel = builder.Build();
            viewModel.CurrentCameraDevice = oldCameraDeviceMock.Object;
            viewModel.CurrentCameraDevice = newCameraDeviceMock.Object;

            Assert.Equal(newCameraDeviceMock.Object, viewModel.CurrentCameraDevice);

            oldCameraDeviceMock.Verify(c => c.StopLiveView(), Times.Once);
            oldCameraDeviceMock.Verify(c => c.Detach(viewModel), Times.Once);
            newCameraDeviceMock.Verify(c => c.Attach(viewModel), Times.Once);
            builder.MockCameraManager.VerifySet(cm => cm.Current = newCameraDeviceMock.Object, Times.Once);

            var oldCameraSettingsMock = oldCameraDeviceMock.As<ICameraDeviceSettings>();
            var newCameraSettingsMock = newCameraDeviceMock.As<ICameraDeviceSettings>();

            oldCameraSettingsMock.VerifyRemove(cs => cs.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>(), Times.Once);
            newCameraSettingsMock.VerifyAdd(cs => cs.PropertyChanged += It.IsAny<PropertyChangedEventHandler>(), Times.Once);
        }

        [Fact]
        public void Notify_ShouldUpdateFrame()
        {
            var viewModel = new CameraSettingsViewModelBuilder().Build();
            // Arrange
            var mat = new Mat();

            // Act
            viewModel.Notify(mat);

            // Assert
            Assert.Equal(mat, viewModel.Frame);
        }

        [Fact]
        public void StartLiveViewCommand_ShouldInvokeStartLiveViewOnCurrentCameraDevice()
        {
            var cameraDeviceMock = new Mock<ICameraDevice>();
            var builder = new CameraSettingsViewModelBuilder().WithCameraSettingsSet(cameraDeviceMock);
            var viewModel = builder.Build();
            // Arrange
            var cameraSettingsList = new List<CameraSettingsEntity>();
            var mockCameraSettingsDbSet = builder.GetQueryableMockDbSet(cameraSettingsList);
            builder.MockDatabaseContext.Setup(db => db.CameraSettings).Returns(mockCameraSettingsDbSet);
            builder.MockDatabaseContext.Setup(db => db.SaveChanges());
            // Act
            viewModel.CurrentCameraDevice = cameraDeviceMock.Object;
            viewModel.StartLiveViewCommand.Execute(null);

            // Assert
            cameraDeviceMock.Verify(c => c.StartLiveView(), Times.Once);
        }

        [Fact]
        public void CameraListChanged_ShouldUpdateAvailables()
        {
            var builder = new CameraSettingsViewModelBuilder();
            var viewModel = builder.Build();
            // Arrange
            var newCameraList = new List<ICameraDevice> { new Mock<ICameraDevice>().Object };

            // Act
            builder.MockCameraManager.Raise(cm => cm.OnAvaliableCameraListChanged += null, newCameraList);

            // Assert
            Assert.Equal(newCameraList, viewModel.Availables);
        }
    }
}
