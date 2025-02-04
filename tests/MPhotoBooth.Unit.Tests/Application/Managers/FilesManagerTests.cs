using MPhotoBooth.Unit.Tests.Application.Managers.Builders;

namespace MPhotoBooth.Unit.Tests.Application.Managers;
public class FilesManagerTests
{
    [Fact]
    public void CopyFile_ShouldCopyFile_WhenFileExists()
    {
        var builder = new FilesManagerBuilder().WithFile("source.txt").WithDirectory("destination");
        // Arrange
        var sourceFilePath = builder.GetFilePath("source.txt");
        var destinationDir = builder.GetDirectoryPath("destination");
        var filesManager = builder.Build();
        // Act
        filesManager.CopyFile(sourceFilePath, destinationDir);

        // Assert
        var destinationFilePath = Path.Combine(destinationDir, "source.txt");
        Assert.True(File.Exists(destinationFilePath));
        builder.Dispose();
    }

    [Fact]
    public void CopyFile_ShouldCreateDirectory_WhenDirectoryDoesNotExist()
    {
        // Arrange
        var builder = new FilesManagerBuilder().WithFile("source.txt");
        var sourceFilePath = builder.GetFilePath("source.txt");
        var destinationDir = Path.Combine(builder.RootDirectory, "destination");
        var filesManager = builder.Build();

        // Act
        filesManager.CopyFile(sourceFilePath, destinationDir);

        // Assert
        Assert.True(Directory.Exists(destinationDir));
        builder.Dispose();
    }

    [Fact]
    public void CopyFile_ShouldNotCopyFile_WhenFileDoesNotExist()
    {
        // Arrange
        var builder = new FilesManagerBuilder();
        var nonExistentFilePath = Path.Combine(builder.RootDirectory, "nonexistent.txt");
        var destinationDir = Path.Combine(builder.RootDirectory, "destination");
        var filesManager = builder.Build();
        // Act
        filesManager.CopyFile(nonExistentFilePath, destinationDir);

        // Assert
        Assert.False(Directory.Exists(destinationDir));
        builder.Dispose();
    }

    [Fact]
    public void DeleteFile_ShouldDeleteFile_WhenFileExists()
    {
        // Arrange
        var builder = new FilesManagerBuilder();
        var filePath = builder.WithFile("fileToDelete.txt").GetFilePath("fileToDelete.txt");
        var filesManager = builder.Build();

        // Act
        filesManager.DeleteFile(filePath);

        // Assert
        Assert.False(File.Exists(filePath));
        builder.Dispose();
    }

    [Fact]
    public void DeleteFile_ShouldNotThrowException_WhenFileDoesNotExist()
    {
        // Arrange
        var builder = new FilesManagerBuilder();
        var nonExistentFilePath = Path.Combine(builder.RootDirectory, "nonexistent.txt");
        var filesManager = builder.Build();

        // Act & Assert
        var exception = Record.Exception(() => filesManager.DeleteFile(nonExistentFilePath));
        Assert.Null(exception);
        builder.Dispose();
    }

    [Fact]
    public void GetFiles_ShouldReturnFiles_WhenDirectoryExists()
    {
        // Arrange
        var builder = new FilesManagerBuilder();
        var filesManager = builder.Build();
        var filePath = builder.WithFile("testfile.txt").GetFilePath("testfile.txt");

        // Act
        var files = filesManager.GetFiles(builder.RootDirectory);

        // Assert
        Assert.Contains(filePath, files);
        builder.Dispose();
    }

    [Fact]
    public void GetFiles_ShouldReturnEmptyList_WhenDirectoryDoesNotExist()
    {
        // Arrange
        var builder = new FilesManagerBuilder();
        var filesManager = builder.Build();
        var nonExistentDir = Path.Combine(builder.RootDirectory, "nonexistent");

        // Act
        var files = filesManager.GetFiles(nonExistentDir);

        // Assert
        Assert.Empty(files);
        builder.Dispose();
    }

    [Fact]
    public void GetFilesNames_ShouldReturnFileNames_WhenDirectoryExists()
    {
        // Arrange
        var builder = new FilesManagerBuilder();
        var filesManager = builder.Build();
        builder.WithFile("testfile.txt");

        // Act
        var fileNames = filesManager.GetFilesNames(builder.RootDirectory);

        // Assert
        Assert.Contains("testfile", fileNames);
        builder.Dispose();
    }

    [Fact]
    public void GetFilesNames_ShouldReturnEmptyList_WhenDirectoryDoesNotExist()
    {
        // Arrange
        var builder = new FilesManagerBuilder();
        var nonExistentDir = Path.Combine(builder.RootDirectory, "nonexistent");
        var filesManager = builder.Build();

        // Act
        var fileNames = filesManager.GetFilesNames(nonExistentDir);

        // Assert
        Assert.Empty(fileNames);
        builder.Dispose();
    }
}
