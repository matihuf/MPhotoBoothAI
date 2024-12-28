using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFilePickerService
{
    Task<byte[]> PickFile(FileTypes fileTypes);
    Task<string> PickFilePath(FileTypes fileTypes);
}
