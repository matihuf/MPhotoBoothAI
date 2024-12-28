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
        int groupId = 1;
        int templateId = 2;
        var thumbnail = CvInvoke.Imread("TestData/square.png");
        var service = _builder.WithUserProfilePath(userProfilePath).WithThumbnail(thumbnail).Build();
        //act
        try
        {
            service.Save(groupId, templateId, "TestData/square.png");
            //assert
            Assert.True(File.Exists(service.GetFullTemplatePath(groupId, templateId)));
            Assert.True(File.Exists(service.GetFullTemplateThumbnailPath(groupId, templateId)));
        }
        finally
        {
            thumbnail.Dispose();
            Directory.Delete(userProfilePath, true);
        }
    }
}
