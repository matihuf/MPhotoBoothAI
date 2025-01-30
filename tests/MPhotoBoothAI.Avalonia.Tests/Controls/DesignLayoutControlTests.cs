using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using Avalonia.Media;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Avalonia.Tests.Builders;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Avalonia.Tests.Controls;
public class DesignLayoutControlTests
{
    [AvaloniaFact]
    public void SwitchToPhotoLayer_ShouldSetActiveLayerSwitchToTrue()
    {
        //arrange
        var control = new DesignLayoutControlBuilder()
            .WithActiveLayerSwitch(false)
            .Build();

        var button = control.FindControl<Button>("PhotoLayerButton");
        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, button));

        Assert.True(control.ActiveLayerSwitch);
    }

    [AvaloniaFact]
    public void SwitchToFrameLayer_ShouldSetActiveLayerSwitchToFalse()
    {
        //arrange
        var control = new DesignLayoutControlBuilder()
            .WithActiveLayerSwitch(true)
            .Build();

        var button = control.FindControl<Button>("FrameLayerButton");
        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, button));

        Assert.False(control.ActiveLayerSwitch);
    }

    [AvaloniaFact]
    public void AddPhoto_ShouldAddPhotoToCanvas()
    {
        //arrange
        var control = new DesignLayoutControlBuilder()
            .WithActiveLayerSwitch(true)
            .Build();

        var photoCanvas = control.FindControl<Canvas>("photoCanvas");
        var addPhotoButton = control.FindControl<Button>("AddPhotoButton");

        //act
        var initialCount = photoCanvas.Children.Count;
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, addPhotoButton));

        //assert
        Assert.Equal(initialCount + 1, photoCanvas.Children.Count);
    }


    [AvaloniaFact]
    public void SaveLayout_ShouldUpdateLayoutData()
    {
        //arrange
        var control = new DesignLayoutControlBuilder().Build();
        control.LayoutData = new LayoutDataEntity();
        control.LayoutFormat = new LayoutFormatEntity
        {
            FormatWidth = 1000,
            FormatRatio = 1.5
        };

        var addPhotoButton = control.FindControl<Button>("AddPhotoButton");
        var saveButton = control.FindControl<Button>("SaveLayoutButton");

        //act
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        saveButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        //assert
        Assert.NotEmpty(control.LayoutData.PhotoLayoutData);
    }

    [AvaloniaFact]
    public void ClearLayout_ShouldRemoveAllItems()
    {
        //arrange
        var control = new DesignLayoutControlBuilder().Build();
        var photoCanvas = control.FindControl<Canvas>("photoCanvas");
        var frameCanvas = control.FindControl<Canvas>("frameCanvas");
        var addPhotoButton = control.FindControl<Button>("AddPhotoButton");
        var clearButton = control.FindControl<Button>("ClearLayoutButton");

        //act
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, addPhotoButton));
        clearButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, clearButton));

        //assert
        Assert.Empty(photoCanvas.Children);
        Assert.Empty(frameCanvas.Children);
    }

    [AvaloniaFact]
    public void AddPhoto_SetCanvasPosition_PhotoShouldMoved()
    {
        //arrange
        var builder = new DesignLayoutControlBuilder();
        var control = builder.Build();
        var layoutFormat = new LayoutFormatEntity
        {
            FormatHeight = 1000,
            FormatRatio = 1,
            FormatWidth = 1000
        };

        var layoutData = new LayoutDataEntity { Id = 1 };
        control.LayoutData = layoutData;
        control.LayoutFormat = layoutFormat;

        var addPhotoButton = control.FindControl<Button>("AddPhotoButton");
        var photoCanvas = control.FindControl<Canvas>("photoCanvas");

        //acr
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, addPhotoButton));
        var photoGrid = photoCanvas.Children[0] as Grid;
        Canvas.SetTop(photoGrid, 200);
        Canvas.SetLeft(photoGrid, 200);
        var saveButton = control.FindControl<Button>("SaveLayoutButton");
        saveButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        //assert
        Assert.NotEqual(layoutData.PhotoLayoutData.First().Left, 0);
        Assert.NotEqual(layoutData.PhotoLayoutData.First().Top, 0);
    }

    [AvaloniaFact]
    public void AddPhoto_SetRotateTransformAngle_PhotoShouldRotate()
    {
        //arrange
        var builder = new DesignLayoutControlBuilder();
        var control = builder.Build();
        var layoutFormat = new LayoutFormatEntity
        {
            FormatHeight = 1000,
            FormatRatio = 1,
            FormatWidth = 1000
        };

        var angle = 30;
        var layoutData = new LayoutDataEntity { Id = 1 };

        control.LayoutData = layoutData;
        control.LayoutFormat = layoutFormat;

        var addPhotoButton = control.FindControl<Button>("AddPhotoButton");
        var photoCanvas = control.FindControl<Canvas>("photoCanvas");

        //act
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, addPhotoButton));

        var photoGrid = photoCanvas.Children[0] as Grid;
        var photoImage = photoGrid.Children[0] as Grid;
        (photoImage.RenderTransform as RotateTransform).Angle = angle;
        var saveButton = control.FindControl<Button>("SaveLayoutButton");
        saveButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        //assert
        Assert.Equal(layoutData.PhotoLayoutData.First().Angle, angle);
    }

}