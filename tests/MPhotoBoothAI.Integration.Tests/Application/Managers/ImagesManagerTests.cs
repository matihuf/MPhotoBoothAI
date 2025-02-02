using MPhotoBoothAI.Application.Managers;

namespace MPhotoBoothAI.Integration.Tests.Application.Managers;
public class ImagesManagerTests
{
    [Fact]
    public void GetImageSizeFromFile_ImageExistShouldReturnSize()
    {
        // arrange
        var imageManager = new ImagesManager();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "testImage_200x200.jpg");

        //act
        var size = imageManager.GetImageSizeFromFile(path);

        //assert
        Assert.NotNull(size);
    }

    [Fact]
    public void GetImageSizeFromFile_ImageNotExistShouldReturnNull()
    {
        // arrange
        var imageManager = new ImagesManager();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");

        //act
        var size = imageManager.GetImageSizeFromFile(path);

        //assert
        Assert.Null(size);
    }

    [Fact]
    public void GetImageSizeFromFile_FileExistButItsNotAnImage_ShouldReturnNull()
    {
        // arrange
        var imageManager = new ImagesManager();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "woman.dat");

        //act
        var size = imageManager.GetImageSizeFromFile(path);

        //assert
        Assert.Null(size);
    }
}
