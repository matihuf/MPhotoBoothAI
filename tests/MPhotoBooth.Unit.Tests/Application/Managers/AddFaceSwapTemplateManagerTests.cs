using Emgu.CV;
using Moq;
using MPhotoBooth.Unit.Tests.Application.Managers.Builders;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Common.Tests;
using MPhotoBoothAI.Models.Entities;
using MPhotoBoothAI.Models.Enums;
using MPhotoBoothAI.Models.FaceSwaps;

namespace MPhotoBooth.Unit.Tests.Application.Managers;
public class AddFaceSwapTemplateManagerTests
{
    private readonly AddFaceSwapTemplateManagerBuilder _builder;

    public AddFaceSwapTemplateManagerTests()
    {
        _builder = new AddFaceSwapTemplateManagerBuilder();
    }

    [Fact]
    public async Task PickTemplate_ShouldCallPickFilePathWithFilePickerFileTypeImage()
    {
        //arrange
        var manager = _builder.Build(new Mock<IDatabaseContext>().Object);
        //act
        using var faceTemplate = await manager.PickTemplate();
        //assert
        _builder.FilePickerService.Verify(x => x.PickFilePath(new[] { FilePickerFileType.Image }));
    }

    [Fact]
    public void SaveTemplate_ShouldSaveToDatabase()
    {
        //arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var group = new FaceSwapTemplateGroupEntity { Name = "groupName" };
        databaseContext.FaceSwapTemplateGroups.Add(group);
        databaseContext.SaveChanges();
        var manager = _builder.Build(databaseContext);
        using var faceSwapTemplate = new FaceSwapTemplate("", 3, Mat.Zeros(1, 1, Emgu.CV.CvEnum.DepthType.Cv8U, 3));
        //act
        int templateId = manager.SaveTemplate(group.Id, faceSwapTemplate);
        //assert
        var dbTemaplte = databaseContext.FaceSwapTemplates.First(x => x.Id == templateId);
        Assert.Equal(faceSwapTemplate.Faces, dbTemaplte.Faces);
        Assert.Equal(group.Id, dbTemaplte.FaceSwapTemplateGroupId);
        Assert.NotEqual(DateTime.Now, dbTemaplte.CreatedAt);
    }
}
