using Microsoft.EntityFrameworkCore;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Models.Entities;
using System.ComponentModel;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels.Builders
{
    public class CameraSettingsViewModelBuilder
    {
        private readonly CameraSettingsViewModel _viewModel;

        public Mock<ICameraManager> MockCameraManager { get; private set; }
        public Mock<IDatabaseContext> MockDatabaseContext { get; private set; }

        public CameraSettingsViewModelBuilder()
        {
            MockCameraManager = new Mock<ICameraManager>();
            MockDatabaseContext = new Mock<IDatabaseContext>();

            // Inicjalizacja pustych list i mocków
            MockCameraManager.Setup(cm => cm.Availables).Returns(new List<ICameraDevice>());

            // Upewnij się, że CameraSettings jest inicjalizowane jako pusta lista, aby uniknąć błędów ArgumentNullException
            var cameraSettingsList = new List<CameraSettingsEntity>();
            var mockCameraSettingsDbSet = GetQueryableMockDbSet(cameraSettingsList);
            MockDatabaseContext.Setup(db => db.CameraSettings).Returns(mockCameraSettingsDbSet);

            _viewModel = new CameraSettingsViewModel(MockCameraManager.Object, MockDatabaseContext.Object);
        }

        public CameraSettingsViewModelBuilder WithMockedICamera(Mock<ICameraDevice> oldCameraDeviceMock, Mock<ICameraDevice> newCameraDeviceMock)
        {
            var oldCameraSettingsMock = oldCameraDeviceMock.As<ICameraDeviceSettings>();
            var newCameraSettingsMock = newCameraDeviceMock.As<ICameraDeviceSettings>();

            oldCameraSettingsMock.SetupAdd(cs => cs.PropertyChanged += It.IsAny<PropertyChangedEventHandler>());
            oldCameraSettingsMock.SetupRemove(cs => cs.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>());

            newCameraSettingsMock.SetupAdd(cs => cs.PropertyChanged += It.IsAny<PropertyChangedEventHandler>());

            oldCameraDeviceMock.Setup(c => c.StopLiveView());
            oldCameraDeviceMock.Setup(c => c.Detach(_viewModel));

            newCameraDeviceMock.Setup(c => c.Attach(_viewModel));
            newCameraDeviceMock.Setup(c => c.CameraName).Returns("NewCamera");

            return this;
        }

        public CameraSettingsViewModelBuilder WithCameraSettingsSet(Mock<ICameraDevice> cameraDeviceMock)
        {
            cameraDeviceMock.Setup(c => c.StartLiveView());
            cameraDeviceMock.Setup(c => c.CameraName).Returns("TestCamera");

            var cameraSettingsMock = cameraDeviceMock.As<ICameraDeviceSettings>();
            cameraSettingsMock.SetupProperty(cs => cs.Iso);
            cameraSettingsMock.SetupProperty(cs => cs.Aperture);
            cameraSettingsMock.SetupProperty(cs => cs.ShutterSpeed);
            cameraSettingsMock.SetupProperty(cs => cs.WhiteBalance);
            return this;
        }

        public CameraSettingsViewModel Build()
        {
            return _viewModel;
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
