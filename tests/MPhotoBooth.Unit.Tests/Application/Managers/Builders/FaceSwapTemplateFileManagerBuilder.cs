using Emgu.CV;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Managers.FaceSwapTemplates;

namespace MPhotoBooth.Unit.Tests.Application.Managers.Builders;
internal class FaceSwapTemplateFileManagerBuilder
{
    private readonly Mock<IApplicationInfoService> _applicationInfoService = new();
    private readonly Mock<IResizeImageService> _resizeImageService = new();

    public IFaceSwapTemplateFileManager Build() => new FaceSwapTemplateFileManager(_applicationInfoService.Object, _resizeImageService.Object);

    internal FaceSwapTemplateFileManagerBuilder WithThumbnail(Mat thumbnail)
    {
        _resizeImageService.Setup(x => x.Resize(It.IsAny<Mat>(), 192, 340, It.IsAny<bool>())).Returns(new MPhotoBoothAI.Models.ResizedImage(thumbnail, 0, 0, 0, 0));
        return this;
    }

    internal FaceSwapTemplateFileManagerBuilder WithUserProfilePath(string userProfilePath)
    {
        _applicationInfoService.Setup(x => x.UserProfilePath).Returns(userProfilePath);
        return this;
    }
}
