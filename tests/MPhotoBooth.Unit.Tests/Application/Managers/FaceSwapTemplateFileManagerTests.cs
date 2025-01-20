using Emgu.CV;
using MPhotoBooth.Unit.Tests.Application.Managers.Builders;

namespace MPhotoBooth.Unit.Tests.Application.Managers;
public class FaceSwapTemplateFileManagerTests
{
    private readonly FaceSwapTemplateFileManagerBuilder _builder;

    public FaceSwapTemplateFileManagerTests()
    {
        _builder = new FaceSwapTemplateFileManagerBuilder();
    }

    [Fact]
    public void Save_SaveTemplateWithThumbnail()
    {
        //arrange
        string userProfilePath = Guid.NewGuid().ToString();
        int groupId = 1;
        int templateId = 2;
        var thumbnail = CvInvoke.Imread("TestData/square.png");
        var manager = _builder.WithUserProfilePath(userProfilePath).WithThumbnail(thumbnail).Build();
        //act
        try
        {
            manager.Save(groupId, templateId, "TestData/square.png");
            //assert
            Assert.True(File.Exists(manager.GetFullTemplatePath(groupId, templateId)));
            Assert.True(File.Exists(manager.GetFullTemplateThumbnailPath(groupId, templateId)));
        }
        finally
        {
            thumbnail.Dispose();
            Directory.Delete(userProfilePath, true);
        }
    }

    [Fact]
    public void DeleteGroup_ShouldDeleteAllFiles()
    {
        //arrange
        string userProfilePath = Guid.NewGuid().ToString();
        int groupId = 1;
        int templateId = 2;
        using var thumbnail = CvInvoke.Imread("TestData/square.png");
        var manager = _builder.WithUserProfilePath(userProfilePath).WithThumbnail(thumbnail).Build();
        manager.Save(groupId, templateId, "TestData/square.png");
        //act
        manager.DeleteGroup(groupId);
        //assert
        Assert.False(Directory.Exists(Directory.GetParent(Directory.GetParent(manager.GetFullTemplatePath(groupId, templateId))?.FullName ?? string.Empty)?.FullName));
        Directory.Delete(userProfilePath, true);
    }

    [Fact]
    public void DeleteTemplate_ShouldDeleteOnlyTemplateFolder()
    {
        //arrange
        string userProfilePath = Guid.NewGuid().ToString();
        int groupId = 1;
        int templateId = 2;
        using var thumbnail = CvInvoke.Imread("TestData/square.png");
        var manager = _builder.WithUserProfilePath(userProfilePath).WithThumbnail(thumbnail).Build();
        manager.Save(groupId, templateId, "TestData/square.png");
        //act
        manager.DeleteTemplate(groupId, templateId);
        //assert
        Assert.False(Directory.Exists(Directory.GetParent(manager.GetFullTemplatePath(groupId, templateId))?.FullName));
        Assert.True(Directory.Exists(Directory.GetParent(Directory.GetParent(manager.GetFullTemplatePath(groupId, templateId))?.FullName ?? string.Empty)?.FullName));
        Directory.Delete(userProfilePath, true);
    }

    [Fact]
    public void GetFullTemplatePath_ShouldBeAsExpected()
    {
        //arrange
        string userProfilePath = Guid.NewGuid().ToString();
        int groupId = 1;
        int templateId = 2;
        var manager = _builder.WithUserProfilePath(userProfilePath).Build();
        //act
        string templatePath = manager.GetFullTemplatePath(groupId, templateId);
        //assert
        Assert.Equal(Path.Combine(userProfilePath, "Templates", groupId.ToString(), templateId.ToString(), $"{templateId}.png"), templatePath);
    }

    [Fact]
    public void GetFullTemplateThumbnailPath_ShouldBeAsExpected()
    {
        //arrange
        string userProfilePath = Guid.NewGuid().ToString();
        int groupId = 1;
        int templateId = 2;
        var manager = _builder.WithUserProfilePath(userProfilePath).Build();
        //act
        string templatePath = manager.GetFullTemplateThumbnailPath(groupId, templateId);
        //assert
        Assert.Equal(Path.Combine(userProfilePath, "Templates", groupId.ToString(), templateId.ToString(), $"{templateId}_thumbnail.png"), templatePath);
    }
}
