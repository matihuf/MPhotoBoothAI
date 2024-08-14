namespace MPhotoBoothAI.Application.Interfaces;

public interface IFilePickerService
{
    Task<byte[]> PickFile();
    Task<string> PickFilePath();
}
