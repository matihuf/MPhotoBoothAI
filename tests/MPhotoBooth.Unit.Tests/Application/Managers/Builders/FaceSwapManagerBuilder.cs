using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Managers;

namespace MPhotoBooth.Unit.Tests.Application.Managers.Builders;

public class FaceSwapManagerBuilder
{
    public readonly Mock<IFaceMaskManager> FaceMaskManager = new();
    public readonly Mock<IFaceSwapPredictService> FaceSwapPredictService = new();
    public readonly Mock<IFaceSwapService> FaceSwapService = new();
    public readonly Mock<IFaceEnhancerService> FaceEnhancerService = new();

    public IFaceSwapManager Build() => new FaceSwapManager(FaceMaskManager.Object, FaceSwapPredictService.Object, FaceSwapService.Object, FaceEnhancerService.Object);
}
