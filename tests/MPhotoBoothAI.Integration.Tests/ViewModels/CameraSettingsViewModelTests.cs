using Emgu.CV;
using Microsoft.EntityFrameworkCore;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Models.Entities;
using System.ComponentModel;

namespace MPhotoBoothAI.Integration.Tests.ViewModels
{
    public class CameraSettingsViewModelTests
    {
        private readonly Mock<ICameraManager> _mockCameraManager;
        private readonly Mock<IDatabaseContext> _mockDatabaseContext;
        private readonly CameraSettingsViewModel _viewModel;

        public CameraSettingsViewModelTests()
        {
            // Arrange mocks
            _mockCameraManager = new Mock<ICameraManager>();
            _mockDatabaseContext = new Mock<IDatabaseContext>();

            _mockCameraManager.Setup(cm => cm.Availables).Returns(new List<ICameraDevice>());

            _viewModel = new CameraSettingsViewModel(_mockCameraManager.Object, _mockDatabaseContext.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Assert
            Assert.NotNull(_viewModel.Frame);
            Assert.Equal(_mockCameraManager.Object.Availables, _viewModel.Availables);
        }

        [Fact]
        public void CurrentCameraDeviceChanged_ShouldUpdateCameraSettings()
        {
            // Arrange
            var oldCameraDeviceMock = new Mock<ICameraDevice>();
            var oldCameraSettingsMock = oldCameraDeviceMock.As<ICameraDeviceSettings>();

            var newCameraDeviceMock = new Mock<ICameraDevice>();
            var newCameraSettingsMock = newCameraDeviceMock.As<ICameraDeviceSettings>();

            oldCameraSettingsMock.SetupRemove(cs => cs.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>());
            newCameraSettingsMock.SetupAdd(cs => cs.PropertyChanged += It.IsAny<PropertyChangedEventHandler>());

            oldCameraDeviceMock.Setup(c => c.StopLiveView());
            oldCameraDeviceMock.Setup(c => c.Detach(_viewModel));

            newCameraDeviceMock.Setup(c => c.Attach(_viewModel));
            newCameraDeviceMock.Setup(c => c.CameraName).Returns("NewCamera");

            var cameraSettingsList = new List<CameraSettingsEntity>();
            var mockCameraSettingsDbSet = GetQueryableMockDbSet(cameraSettingsList);
            _mockDatabaseContext.Setup(db => db.CameraSettings).Returns(mockCameraSettingsDbSet);

            _viewModel.CurrentCameraDevice = oldCameraDeviceMock.Object;

            // Act
            _viewModel.CurrentCameraDevice = newCameraDeviceMock.Object;

            // Assert
            Assert.Equal(newCameraDeviceMock.Object, _viewModel.CurrentCameraDevice);
            oldCameraSettingsMock.VerifyRemove(cs => cs.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>(), Times.Once);
            newCameraSettingsMock.VerifyAdd(cs => cs.PropertyChanged += It.IsAny<PropertyChangedEventHandler>(), Times.Once);
            oldCameraDeviceMock.Verify(c => c.StopLiveView(), Times.Once);
            oldCameraDeviceMock.Verify(c => c.Detach(_viewModel), Times.Once);
            newCameraDeviceMock.Verify(c => c.Attach(_viewModel), Times.Once);
            _mockCameraManager.VerifySet(cm => cm.Current = newCameraDeviceMock.Object, Times.Once);
        }

        [Fact]
        public void Notify_ShouldUpdateFrame()
        {
            // Arrange
            var mat = new Mat();

            // Act
            _viewModel.Notify(mat);

            // Assert
            Assert.Equal(mat, _viewModel.Frame);
        }

        [Fact]
        public void StartLiveViewCommand_ShouldInvokeStartLiveViewOnCurrentCameraDevice()
        {
            // Arrange
            var cameraDeviceMock = new Mock<ICameraDevice>();
            cameraDeviceMock.Setup(c => c.StartLiveView());
            cameraDeviceMock.Setup(c => c.CameraName).Returns("TestCamera");

            var cameraSettingsMock = cameraDeviceMock.As<ICameraDeviceSettings>();
            cameraSettingsMock.SetupProperty(cs => cs.Iso);
            cameraSettingsMock.SetupProperty(cs => cs.Aperture);
            cameraSettingsMock.SetupProperty(cs => cs.ShutterSpeed);
            cameraSettingsMock.SetupProperty(cs => cs.WhiteBalance);

            var cameraSettingsList = new List<CameraSettingsEntity>();
            var mockCameraSettingsDbSet = GetQueryableMockDbSet(cameraSettingsList);
            _mockDatabaseContext.Setup(db => db.CameraSettings).Returns(mockCameraSettingsDbSet);
            _mockDatabaseContext.Setup(db => db.SaveChanges());

            // Act
            _viewModel.CurrentCameraDevice = cameraDeviceMock.Object;
            _viewModel.StartLiveViewCommand.Execute(null);

            // Assert
            cameraDeviceMock.Verify(c => c.StartLiveView(), Times.Once);
        }

        [Fact]
        public void CameraListChanged_ShouldUpdateAvailables()
        {
            // Arrange
            var newCameraList = new List<ICameraDevice> { new Mock<ICameraDevice>().Object };

            // Act
            _mockCameraManager.Raise(cm => cm.OnAvaliableCameraListChanged += null, newCameraList);

            // Assert
            Assert.Equal(newCameraList, _viewModel.Availables);
        }

        public DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(s => sourceList.Add(s));

            return mockSet.Object;
        }
    }
}
