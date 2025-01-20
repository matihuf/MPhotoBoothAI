using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.Services;
public class FilesManager : IFilesManager
{
    public void CopyFile(string filePath, string dirPath, string? fileName = null)
    {
        if (!File.Exists(filePath))
        {
            return;
        }
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.Copy(filePath, Path.Combine(dirPath, String.IsNullOrEmpty(fileName) ? Path.GetFileName(filePath) : fileName), true);
    }


    public void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public IEnumerable<string> GetFiles(string path)
    {
        if (!Directory.Exists(path))
        {
            return [];
        }
        return Directory.GetFiles(path);
    }

    public IEnumerable<string> GetFilesNames(string path)
    {
        var filesList = new List<string>();
        foreach (var file in GetFiles(path))
        {
            filesList.Add(Path.GetFileNameWithoutExtension(file));
        }
        return filesList;
    }
}
