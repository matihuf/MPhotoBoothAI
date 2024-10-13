using MPhotoBoothAI.Application.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MPhotoBoothAI.Infrastructure.Services
{
    public class CameraManager : ICameraManager, INotifyPropertyChanged
    {
        private readonly IEnumerable<ICameraDevice> _cameras;

        public CameraManager(IEnumerable<ICameraDevice> cameras)
        {
            _cameras = cameras;
            foreach (ICameraDevice camera in _cameras)
            {
                camera.Connected += Camera_AvilableChanged;
                camera.Disconnected += Camera_AvilableChanged;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public IEnumerable<ICameraDevice> Availables => _cameras != null ? _cameras.Where(x => x.IsAvailable) : [];

        public ICameraDevice Current { get; set; }

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

        private void Camera_AvilableChanged(object? sender, EventArgs e) => NotifyPropertyChanged(nameof(Availables));

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
