using MPhotoBoothAI.Models.Enums;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFilePickerService
{
    Task<byte[]> PickFile(FilePickerFileType[]? filePickerFileTypes = null);
    Task<string> PickFilePath(FilePickerFileType[]? filePickerFileTypes = null);
}
