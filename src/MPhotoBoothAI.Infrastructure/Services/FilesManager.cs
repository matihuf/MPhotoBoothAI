using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.Services;
public class FilesManager : IFilesManager
{
    public void CopyFile(string filePath, string dirPath
        )
    {
        if (!File.Exists(filePath))
        {
            return;
        }
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        var fileName = Path.GetFileName(filePath);
        File.Copy(filePath, Path.Combine(dirPath, fileName), true);
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
