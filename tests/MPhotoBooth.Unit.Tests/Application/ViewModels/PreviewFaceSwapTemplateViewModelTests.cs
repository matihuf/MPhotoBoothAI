using Emgu.CV;
using Moq;
using MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
using MPhotoBoothAI.Models.WindowParameters;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels;
public class PreviewFaceSwapTemplateViewModelTests
{
    private readonly PreviewFaceSwapTemplateViewModelBuilder _builder;

    public PreviewFaceSwapTemplateViewModelTests()
    {
        _builder = new PreviewFaceSwapTemplateViewModelBuilder();
    }

    [Fact]
    public void SetParameters_SetPreview()
    {
        //arrange
        var viewModel = _builder.Build();
        //act
        viewModel.Parameters = new PreviewFaceSwapTemplateParameters(1, 1, "TestData/square.png", 3);
        //assert
        Assert.NotNull(viewModel.Preview);
    }

    [Fact]
    public void SetParameters_MarkFaces()
    {
        //arrange
        var viewModel = _builder.Build();
        //act
        viewModel.Parameters = new PreviewFaceSwapTemplateParameters(1, 1, "TestData/square.png", 3);
        //assert
        _builder.FaceDetectionManager.Verify(x => x.Mark(It.IsAny<Mat>()));
    }
}
