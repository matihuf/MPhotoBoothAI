namespace MPhotoBoothAI.Application.Interfaces;
public interface IMainWindow
{
    bool IsEnabled { get; set; }
    void Close();
}
