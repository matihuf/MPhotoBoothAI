using Emgu.CV;
using MPhotoBooth.Unit.Tests.Infrastructure.Services.Builders;

namespace MPhotoBooth.Unit.Tests.Infrastructure.Services;
public class FaceSwapTemplateFileServiceTests
{
    private readonly FaceSwapTemplateFileServiceBuilder _builder;

    public FaceSwapTemplateFileServiceTests()
    {
        _builder = new FaceSwapTemplateFileServiceBuilder();
    }

    [Fact]
    public void Save_SaveTemplateWithThumbnail()
    {
        //arrange
        string userProfilePath = Guid.NewGuid().ToString();
        string groupName = Guid.NewGuid().ToString();
        string templateId = Guid.NewGuid().ToString();
        var thumbnail = CvInvoke.Imread("TestData/square.png");
        var service = _builder.WithUserProfilePath(userProfilePath).WithThumbnail(thumbnail).Build();
        //act
        try
        {
            string resultPath = service.Save(groupName, templateId, "TestData/square.png");
            //assert
            Assert.True(File.Exists(resultPath));
            Assert.True(File.Exists($"{resultPath.Split('.')[0]}_thumbnail{Path.GetExtension(resultPath)}"));
        }
        finally
        {
            thumbnail.Dispose();
            Directory.Delete(userProfilePath, true);
        }
    }
}
