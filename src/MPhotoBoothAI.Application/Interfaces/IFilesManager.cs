namespace MPhotoBoothAI.Application.Interfaces;
public interface IFilesManager
{
    void CopyFile(string filePath, string dirPath);

    void DeleteFile(string path);

    IEnumerable<string> GetFiles(string path);

    IEnumerable<string> GetFilesNames(string path);
}
