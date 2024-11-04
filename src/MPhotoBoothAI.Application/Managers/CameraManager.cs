using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Application.Managers
{
    public class CameraManager : ICameraManager
    {
        private readonly IEnumerable<ICameraDevice> _cameras;

        public IEnumerable<ICameraDevice> Availables => _cameras != null ? _cameras.Where(x => x.IsAvailable) : [];

        public ICameraDevice? Current { get; set; }

        public event ICameraManager.AvaliableCameraListChanged OnAvaliableCameraListChanged;

        public CameraManager(IEnumerable<ICameraDevice> cameras)
        {
            _cameras = cameras;
            foreach (ICameraDevice camera in _cameras)
            {
                camera.Connected += Camera_AvilableChanged;
                camera.Disconnected += Camera_AvilableChanged;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (ICameraDevice camera in _cameras)
                {
                    camera.Connected -= Camera_AvilableChanged;
                    camera.Disconnected -= Camera_AvilableChanged;
                }
            }
        }

        private void Camera_AvilableChanged(object? sender, EventArgs e) => NotifyCameraListChanged(Availables);

        private void NotifyCameraListChanged(IEnumerable<ICameraDevice> cameraList)
        {
            OnAvaliableCameraListChanged?.Invoke(cameraList);
        }
    }
}
