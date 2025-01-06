using Emgu.CV;
using Emgu.CV.CvEnum;
using Moq;
using MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.FaceSwaps;
using MPhotoBoothAI.Models.WindowParameters;
using System.ComponentModel;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels;
public class AddFaceSwapTemplateViewModelTests
{
    private readonly AddFaceSwapTemplateViewModelBuilder _builder;

    public AddFaceSwapTemplateViewModelTests()
    {
        _builder = new AddFaceSwapTemplateViewModelBuilder();
    }

    [Fact]
    public async Task PickTemplate_ClearImage()
    {
        //arrange
        var viewModel = _builder.Build();
        viewModel.Image = Mat.Zeros(1, 1, DepthType.Cv8U, 3);
        //act
        await viewModel.PickTemplateCommand.ExecuteAsync(null);
        //assert
        Assert.Null(viewModel.Image);
    }

    [Fact]
    public async Task PickTemplate_SetIsFaceDetectionProgressActiveToTrueFinallyFalse()
    {
        //arrange
        var expected = new[] { true, false };
        var viewModel = _builder.Build();
        var propertyChanges = new List<bool>(2);
        void Handler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.IsFaceDetectionProgressActive))
            {
                propertyChanges.Add(viewModel.IsFaceDetectionProgressActive);
            }
        }
        viewModel.PropertyChanged += Handler!;
        //act
        try
        {
            await viewModel.PickTemplateCommand.ExecuteAsync(null);
        }
        finally
        {
            viewModel.PropertyChanged -= Handler!;
        }
        //assert
        Assert.Equal(expected, propertyChanges);
    }

    [Fact]
    public async Task PickTemplate_ClearFaceSwapTemplateThenSet()
    {
        //arrange
        var faceSwapTemplate = new FaceSwapTemplate(string.Empty, 1, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        var viewModel = _builder.WithFaceSwapTemplate(faceSwapTemplate).Build();
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        var propertyChanges = new List<FaceSwapTemplate?>(2);
        void Handler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.FaceSwapTemplate))
            {
                propertyChanges.Add(viewModel.FaceSwapTemplate);
            }
        }
        viewModel.PropertyChanged += Handler!;
        //act
        try
        {
            await viewModel.PickTemplateCommand.ExecuteAsync(null);
        }
        finally
        {
            viewModel.PropertyChanged -= Handler!;
        }
        //assert
        Assert.Equal(new[] { null, faceSwapTemplate }, propertyChanges);
    }

    [Fact]
    public async Task Swap_ClearImage()
    {
        //arrange
        using var faceSwapTemplate = new FaceSwapTemplate("TestData/square.png", 1, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        var viewModel = _builder.Build();
        viewModel.Image = Mat.Zeros(1, 1, DepthType.Cv8U, 3);
        using var cameraFrame = Mat.Zeros(1, 1, DepthType.Cv8U, 3);
        viewModel.CameraFrame = cameraFrame;
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        //act
        await viewModel.SwapCommand.ExecuteAsync(null);
        //assert
        Assert.Null(viewModel.Image);
    }

    [Fact]
    public async Task Swap_SetIsEnabledToFalseFinallyTrue()
    {
        //arrange
        var expected = new[] { false, true };
        using var faceSwapTemplate = new FaceSwapTemplate("TestData/square.png", 1, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        var viewModel = _builder.Build();
        using var cameraFrame = Mat.Zeros(1, 1, DepthType.Cv8U, 3);
        viewModel.CameraFrame = cameraFrame;
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        var propertyChanges = new List<bool>(2);
        void Handler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.IsEnabled))
            {
                propertyChanges.Add(viewModel.IsEnabled);
            }
        }
        viewModel.PropertyChanged += Handler!;
        //act
        try
        {
            await viewModel.SwapCommand.ExecuteAsync(null);
        }
        finally
        {
            viewModel.PropertyChanged -= Handler!;
        }
        //assert
        Assert.Equal(expected, propertyChanges);
    }

    [Fact]
    public async Task Swap_SetIsProgressActiveToTrueFinallyFalse()
    {
        //arrange
        var expected = new[] { true, false };
        using var faceSwapTemplate = new FaceSwapTemplate("TestData/square.png", 1, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        var viewModel = _builder.Build();
        using var cameraFrame = Mat.Zeros(1, 1, DepthType.Cv8U, 3);
        viewModel.CameraFrame = cameraFrame;
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        var propertyChanges = new List<bool>(2);
        void Handler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.IsProgressActive))
            {
                propertyChanges.Add(viewModel.IsProgressActive);
            }
        }
        viewModel.PropertyChanged += Handler!;
        //act
        try
        {
            await viewModel.SwapCommand.ExecuteAsync(null);
        }
        finally
        {
            viewModel.PropertyChanged -= Handler!;
        }
        //assert
        Assert.Equal(expected, propertyChanges);
    }

    [Fact]
    public async Task Swap_SeTemplateImageOpacityTo03Finally1()
    {
        //arrange
        var expected = new[] { 0.3, 1 };
        using var faceSwapTemplate = new FaceSwapTemplate("TestData/square.png", 1, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        var viewModel = _builder.Build();
        using var cameraFrame = Mat.Zeros(1, 1, DepthType.Cv8U, 3);
        viewModel.CameraFrame = cameraFrame;
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        var propertyChanges = new List<double>(2);
        void Handler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.TemplateImageOpacity))
            {
                propertyChanges.Add(viewModel.TemplateImageOpacity);
            }
        }
        viewModel.PropertyChanged += Handler!;
        //act
        try
        {
            await viewModel.SwapCommand.ExecuteAsync(null);
        }
        finally
        {
            viewModel.PropertyChanged -= Handler!;
        }
        //assert
        Assert.Equal(expected, propertyChanges);
    }

    [Fact]
    public async Task Swap_CallFaceMultiSwapManager_Swap()
    {
        //arrange
        using var faceSwapTemplate = new FaceSwapTemplate("TestData/square.png", 1, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        var viewModel = _builder.Build();
        using var cameraFrame = Mat.Zeros(1, 1, DepthType.Cv8U, 3);
        viewModel.CameraFrame = cameraFrame;
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        //act
        await viewModel.SwapCommand.ExecuteAsync(null);
        //assert
        _builder.FaceMultiSwapManager.Verify(x => x.Swap(It.IsAny<Mat>(), It.IsAny<Mat>()));
    }

    [Fact]
    public void Save_CalldFaceSwapTemplateManager_SaveTemplate()
    {
        //arrange
        using var faceSwapTemplate = new FaceSwapTemplate("TestData/square.png", 1, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        var viewModel = _builder.Build();
        viewModel.Parameters = new AddFaceSwapTemplateParameters(1);
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        var mainWindow = new Mock<IMainWindow>();
        //act
        viewModel.SaveCommand.Execute(mainWindow.Object);
        //assert
        _builder.AddFaceSwapTemplateManager.Verify(x => x.SaveTemplate(viewModel.Parameters.GroupId, faceSwapTemplate));
    }

    [Fact]
    public void Save_SetResult()
    {
        //arrange
        using var faceSwapTemplate = new FaceSwapTemplate("TestData/square.png", 1, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        int templateId = 2;
        var viewModel = _builder.WithSaveTemplate(templateId).Build();
        viewModel.Parameters = new AddFaceSwapTemplateParameters(1);
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        var mainWindow = new Mock<IMainWindow>();
        //act
        viewModel.SaveCommand.Execute(mainWindow.Object);
        //assert
        Assert.NotNull(viewModel.Result);
        Assert.Equal(faceSwapTemplate.Faces, viewModel.Result.Faces);
        Assert.Equal(templateId, viewModel.Result.TemplateId);
    }

    [Fact]
    public void Save_CloseWindow()
    {
        //arrange
        using var faceSwapTemplate = new FaceSwapTemplate("TestData/square.png", 1, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        int templateId = 2;
        var viewModel = _builder.WithSaveTemplate(templateId).Build();
        viewModel.Parameters = new AddFaceSwapTemplateParameters(1);
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        var mainWindow = new Mock<IMainWindow>();
        //act
        viewModel.SaveCommand.Execute(mainWindow.Object);
        //assert
        mainWindow.Verify(x => x.Close());
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    public void OnFaceSwapTemplateChanged_SaveButtonIsEnabled_WhenFacesDetected(int faces, bool expected)
    {
        //arrange
        using var faceSwapTemplate = new FaceSwapTemplate("TestData/square.png", faces, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        var viewModel = _builder.Build();
        //act
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        //assert
        Assert.Equal(expected, viewModel.SaveButtonIsEnabled);
    }

    [Theory]
    [InlineData(0, true, false)]
    [InlineData(1, false, false)]
    [InlineData(1, true, true)]
    public void OnFaceSwapTemplateChanged_SwapButtonIsEnabled_WhenFacesDetectedAndCameraFrameIsNotNull(int faces, bool cameraFrameIsNotNull, bool expected)
    {
        //arrange
        using var faceSwapTemplate = new FaceSwapTemplate("TestData/square.png", faces, Mat.Zeros(1, 1, DepthType.Cv8U, 3));
        var viewModel = _builder.Build();
        using var cameraFrame = Mat.Zeros(1, 1, DepthType.Cv8U, 3);
        viewModel.CameraFrame = cameraFrameIsNotNull ? cameraFrame : null;
        //act
        viewModel.FaceSwapTemplate = faceSwapTemplate;
        //assert
        Assert.Equal(expected, viewModel.SwapButtonIsEnabled);
    }
}
