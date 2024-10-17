using Emgu.CV;
using Emgu.CV.CvEnum;
using Moq;
using MPhotoBooth.Unit.Tests.Application.Managers.Builders;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBooth.Unit.Tests.Application.Managers;

public class FaceMultiSwapManagerTests
{
    private readonly FaceMultiSwapManagerBuilder _builder;

    public FaceMultiSwapManagerTests()
    {
        _builder = new FaceMultiSwapManagerBuilder();
    }

    [Theory]
    [InlineData(Gender.Male, Gender.Male)]
    [InlineData(Gender.Female, Gender.Female)]
    [InlineData(Gender.Male, Gender.Female)]
    [InlineData(Gender.Female, Gender.Male)]
    public void Swap_OneSourceOneTarget_OneSwap(Gender sourceGender, Gender targetGender)
    {
        //arrange
        using var source = new Mat(5, 5, DepthType.Default, 1);
        using var target = new Mat(10, 10, DepthType.Default, 1);

        using var sourceAlign = new FaceAlignDetails(new Mat(), new Mat(1, 1, DepthType.Default, 1), sourceGender);
        using var targetAlign = new FaceAlignDetails(new Mat(), new Mat(2, 2, DepthType.Default, 1), targetGender);

        var manager = _builder.WithAligns(source, [sourceAlign]).WithAligns(target, [targetAlign]).Build();
        //act
        using var swap = manager.Swap(source, target);
        //assert
        _builder.FaceSwapManager.Verify(x => x.Swap(sourceAlign, targetAlign, It.IsAny<Mat>()), Times.Once());
    }

    [Fact]
    public void Swap_OneSourceTwoTargets_OneSwapByGender()
    {
        //arrange
        using var source = new Mat(5, 5, DepthType.Default, 1);
        using var target = new Mat(10, 10, DepthType.Default, 1);
        var sourceGender = Gender.Male;

        using var sourceAlign = new FaceAlignDetails(new Mat(), new Mat(1, 1, DepthType.Default, 1), sourceGender);
        var targetAligns = new[]
        {
            new FaceAlignDetails(new Mat(), new Mat(2, 2, DepthType.Default, 1), Gender.Female),
            new FaceAlignDetails(new Mat(), new Mat(3, 3, DepthType.Default, 1), sourceGender)
        };

        var manager = _builder.WithAligns(source, [sourceAlign]).WithAligns(target, targetAligns).Build();
        //act
        using var swap = manager.Swap(source, target);
        //assert
        _builder.FaceSwapManager.Verify(x => x.Swap(sourceAlign, targetAligns[1], It.IsAny<Mat>()), Times.Once());
    }

    [Fact]
    public void Swap_OneSourceTwoTargetsWithDifferentGenders_OneSwapWithFirstTarget()
    {
        //arrange
        using var source = new Mat(5, 5, DepthType.Default, 1);
        using var target = new Mat(10, 10, DepthType.Default, 1);
        var sourceGender = Gender.Male;

        using var sourceAlign = new FaceAlignDetails(new Mat(), new Mat(1, 1, DepthType.Default, 1), sourceGender);
        var targetAligns = new[]
        {
            new FaceAlignDetails(new Mat(), new Mat(2, 2, DepthType.Default, 1), Gender.Female),
            new FaceAlignDetails(new Mat(), new Mat(3, 3, DepthType.Default, 1), Gender.Female)
        };

        var manager = _builder.WithAligns(source, [sourceAlign]).WithAligns(target, targetAligns).Build();
        //act
        using var swap = manager.Swap(source, target);
        //assert
        _builder.FaceSwapManager.Verify(x => x.Swap(sourceAlign, targetAligns[0], It.IsAny<Mat>()), Times.Once());
    }

    [Fact]
    public void Swap_TwoSourcesTwoTargets_TwoSwapsByGenders()
    {
        //arrange
        using var source = new Mat(5, 5, DepthType.Default, 1);
        using var target = new Mat(10, 10, DepthType.Default, 1);

        var sourceAligns = new[]
        {
            new FaceAlignDetails(new Mat(), new Mat(1, 1, DepthType.Default, 1), Gender.Male),
            new FaceAlignDetails(new Mat(), new Mat(4, 4, DepthType.Default, 1), Gender.Female)
        };
        var targetAligns = new[]
        {
            new FaceAlignDetails(new Mat(), new Mat(2, 2, DepthType.Default, 1), Gender.Female),
            new FaceAlignDetails(new Mat(), new Mat(3, 3, DepthType.Default, 1), Gender.Male)
        };

        var manager = _builder.WithAligns(source, sourceAligns).WithAligns(target, targetAligns).Build();
        //act
        using var swap = manager.Swap(source, target);
        //assert
        _builder.FaceSwapManager.Verify(x => x.Swap(sourceAligns[0], targetAligns[1], It.IsAny<Mat>()), Times.Once());
        _builder.FaceSwapManager.Verify(x => x.Swap(sourceAligns[1], targetAligns[0], It.IsAny<Mat>()), Times.Once());
    }

    [Fact]
    public void Swap_TwoFemaleSourcesTwoMaleTargets_TwoSwapsByIndex()
    {
        //arrange
        using var source = new Mat(5, 5, DepthType.Default, 1);
        using var target = new Mat(10, 10, DepthType.Default, 1);

        var sourceAligns = new[]
        {
            new FaceAlignDetails(new Mat(), new Mat(1, 1, DepthType.Default, 1), Gender.Female),
            new FaceAlignDetails(new Mat(), new Mat(4, 4, DepthType.Default, 1), Gender.Female)
        };
        var targetAligns = new[]
        {
            new FaceAlignDetails(new Mat(), new Mat(2, 2, DepthType.Default, 1), Gender.Male),
            new FaceAlignDetails(new Mat(), new Mat(3, 3, DepthType.Default, 1), Gender.Male)
        };

        var manager = _builder.WithAligns(source, sourceAligns).WithAligns(target, targetAligns).Build();
        //act
        using var swap = manager.Swap(source, target);
        //assert
        _builder.FaceSwapManager.Verify(x => x.Swap(sourceAligns[0], targetAligns[0], It.IsAny<Mat>()), Times.Once());
        _builder.FaceSwapManager.Verify(x => x.Swap(sourceAligns[1], targetAligns[1], It.IsAny<Mat>()), Times.Once());
    }
}
