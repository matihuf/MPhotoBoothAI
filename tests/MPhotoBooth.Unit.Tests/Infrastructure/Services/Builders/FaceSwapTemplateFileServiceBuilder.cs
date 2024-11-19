using Emgu.CV;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Infrastructure.Services;

namespace MPhotoBooth.Unit.Tests.Infrastructure.Services.Builders;
internal class FaceSwapTemplateFileServiceBuilder
{
    private readonly Mock<IApplicationInfoService> _applicationInfoService = new();
    private readonly Mock<IResizeImageService> _resizeImageService = new();

    public IFaceSwapTemplateFileService Build() => new FaceSwapTemplateFileService(_applicationInfoService.Object, _resizeImageService.Object);

    internal FaceSwapTemplateFileServiceBuilder WithThumbnail(Mat thumbnail)
    {
        _resizeImageService.Setup(x => x.GetThumbnail(It.IsAny<Mat>(), It.IsAny<float>())).Returns(thumbnail);
        return this;
    }

    internal FaceSwapTemplateFileServiceBuilder WithUserProfilePath(string userProfilePath)
    {
        _applicationInfoService.Setup(x => x.UserProfilePath).Returns(userProfilePath);
        return this;
    }
}
