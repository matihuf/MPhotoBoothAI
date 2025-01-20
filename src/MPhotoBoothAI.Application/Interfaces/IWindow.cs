namespace MPhotoBoothAI.Application.Interfaces;
public interface IWindow
{
    bool IsEnabled { get; set; }
    void Close();
}
