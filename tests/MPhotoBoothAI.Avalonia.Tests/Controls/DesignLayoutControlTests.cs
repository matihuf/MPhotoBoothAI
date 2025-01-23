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
        var control = new DesignLayoutControlBuilder()
            .WithActiveLayerSwitch(true)
            .Build();

        var photoCanvas = control.FindControl<Canvas>("photoCanvas");
        var initialCount = photoCanvas.Children.Count;

        var addPhotoButton = control.FindControl<Button>("AddPhotoButton");
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, addPhotoButton));

        Assert.Equal(initialCount + 1, photoCanvas.Children.Count);
    }

    [AvaloniaFact]
    public void RemovePhoto_ShouldRemoveLastPhotoFromCanvas()
    {
        var control = new DesignLayoutControlBuilder()
            .WithActiveLayerSwitch(true)
            .Build();

        var photoCanvas = control.FindControl<Canvas>("photoCanvas");

        var addPhotoButton = control.FindControl<Button>("AddPhotoButton");
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, addPhotoButton));
        var initialCount = photoCanvas.Children.Count;

        var removePhotoButton = control.FindControl<Button>("RemovePhotoButton");
        removePhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, removePhotoButton));

        Assert.Equal(initialCount - 1, photoCanvas.Children.Count);
    }

    [AvaloniaFact]
    public void SaveLayout_ShouldUpdateLayoutData()
    {
        var control = new DesignLayoutControlBuilder().Build();
        control.LayoutData = new LayoutDataEntity();
        control.LayoutFormat = new LayoutFormatEntity
        {
            FormatWidth = 1000,
            FormatRatio = 1.5
        };

        var addPhotoButton = control.FindControl<Button>("AddPhotoButton");
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        var saveButton = control.FindControl<Button>("SaveLayoutButton");
        saveButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.NotEmpty(control.LayoutData.PhotoLayoutData);
    }

    [AvaloniaFact]
    public void ClearLayout_ShouldRemoveAllItems()
    {
        var control = new DesignLayoutControlBuilder().Build();

        var photoCanvas = control.FindControl<Canvas>("photoCanvas");
        var frameCanvas = control.FindControl<Canvas>("frameCanvas");

        var addPhotoButton = control.FindControl<Button>("AddPhotoButton");
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, addPhotoButton));

        var clearButton = control.FindControl<Button>("ClearLayoutButton");
        clearButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, clearButton));

        Assert.Empty(photoCanvas.Children);
        Assert.Empty(frameCanvas.Children);
    }

    [AvaloniaFact]
    public void AddPhoto_SetCanvasPosition_PhotoShouldMoved()
    {
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
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, addPhotoButton));

        var photoCanvas = control.FindControl<Canvas>("photoCanvas");

        var photoGrid = photoCanvas.Children[0] as Grid;
        Canvas.SetTop(photoGrid, 200);
        Canvas.SetLeft(photoGrid, 200);
        var saveButton = control.FindControl<Button>("SaveLayoutButton");
        saveButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.NotEqual(layoutData.PhotoLayoutData.First().Left, 0);
        Assert.NotEqual(layoutData.PhotoLayoutData.First().Top, 0);
    }

    [AvaloniaFact]
    public void AddPhoto_SetRotateTransformAngle_PhotoShouldRotate()
    {
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
        addPhotoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, addPhotoButton));

        var photoCanvas = control.FindControl<Canvas>("photoCanvas");

        var photoGrid = photoCanvas.Children[0] as Grid;
        var photoImage = photoGrid.Children[0] as Grid;
        (photoImage.RenderTransform as RotateTransform).Angle = angle;
        var saveButton = control.FindControl<Button>("SaveLayoutButton");
        saveButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal(layoutData.PhotoLayoutData.First().Angle, angle);
    }

}