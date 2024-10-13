
namespace MPhotoBoothAI.Application.Interfaces
{
    public interface ICameraManager : IDisposable
    {
        IEnumerable<ICameraDevice> Availables { get; }
        ICameraDevice Current { get; set; }
    }
}
