using Emgu.CV;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Infrastructure.Services;

namespace MPhotoBooth.Unit.Tests.Application.Managers.Builders;
internal class FaceSwapTemplateFileManagerBuilder
{
    private readonly Mock<IApplicationInfoService> _applicationInfoService = new();
    private readonly Mock<IResizeImageService> _resizeImageService = new();

    public IFaceSwapTemplateFileManager Build() => new FaceSwapTemplateFileManager(_applicationInfoService.Object, _resizeImageService.Object);

    internal FaceSwapTemplateFileManagerBuilder WithThumbnail(Mat thumbnail)
    {
        _resizeImageService.Setup(x => x.GetThumbnail(It.IsAny<Mat>(), It.IsAny<float>())).Returns(thumbnail);
        return this;
    }

    internal FaceSwapTemplateFileManagerBuilder WithUserProfilePath(string userProfilePath)
    {
        _applicationInfoService.Setup(x => x.UserProfilePath).Returns(userProfilePath);
        return this;
    }
}
