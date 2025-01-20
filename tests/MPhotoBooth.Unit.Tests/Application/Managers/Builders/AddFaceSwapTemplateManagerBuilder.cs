using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Managers.FaceSwapTemplates;
using MPhotoBoothAI.Models.Enums;

namespace MPhotoBooth.Unit.Tests.Application.Managers.Builders;
internal class AddFaceSwapTemplateManagerBuilder
{
    public readonly Mock<IFilePickerService> FilePickerService = new();
    private readonly Mock<IFaceDetectionManager> _faceDetectionManager = new();
    private readonly Mock<IFaceSwapTemplateFileManager> _faceSwapTemplateFileManager = new();

    public AddFaceSwapTemplateManagerBuilder()
    {
        FilePickerService.Setup(x => x.PickFilePath(new[] { FilePickerFileType.Image })).ReturnsAsync("TestData/square.png");
    }

    public IAddFaceSwapTemplateManager Build(IDatabaseContext databaseContext) => new AddFaceSwapTemplateManager(FilePickerService.Object, _faceDetectionManager.Object,
        _faceSwapTemplateFileManager.Object, databaseContext);
}
