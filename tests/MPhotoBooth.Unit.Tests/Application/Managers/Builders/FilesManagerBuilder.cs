using MPhotoBoothAI.Application.Managers;

namespace MPhotoBooth.Unit.Tests.Application.Managers.Builders;
public class FilesManagerBuilder : IDisposable
{
    public string RootDirectory { get; private set; }

    public FilesManagerBuilder()
    {
        RootDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(RootDirectory);
    }

    public FilesManagerBuilder WithFile(string fileName, string content = "")
    {
        var filePath = Path.Combine(RootDirectory, fileName);
        File.WriteAllText(filePath, content);
        return this;
    }

    public FilesManagerBuilder WithDirectory(string directoryName)
    {
        var directoryPath = Path.Combine(RootDirectory, directoryName);
        Directory.CreateDirectory(directoryPath);
        return this;
    }

    public FilesManager Build()
    {
        return new FilesManager();
    }

    public string GetFilePath(string fileName)
    {
        return Path.Combine(RootDirectory, fileName);
    }

    public string GetDirectoryPath(string directoryName)
    {
        return Path.Combine(RootDirectory, directoryName);
    }

    public void Dispose()
    {
        if (Directory.Exists(RootDirectory))
        {
            Directory.Delete(RootDirectory, true);
        }
    }
}
