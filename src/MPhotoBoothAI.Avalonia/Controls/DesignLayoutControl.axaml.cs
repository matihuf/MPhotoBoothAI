using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;

namespace MPhotoBoothAI.Avalonia.Controls;

public partial class DesignLayoutControl : UserControl
{
    private const double StartScale = 0.25;

    public static readonly StyledProperty<double> CanvasWidthProperty =
        AvaloniaProperty.Register<DesignLayoutControl, double>(nameof(CanvasWidth));

    public double CanvasWidth
    {
        get => this.GetValue(CanvasWidthProperty);
        set => SetValue(CanvasWidthProperty, value);
    }

    public static readonly StyledProperty<double> CanvasHeightProperty =
        AvaloniaProperty.Register<DesignLayoutControl, double>(nameof(CanvasHeight));

    public double CanvasHeight
    {
        get => this.GetValue(CanvasHeightProperty);
        set => SetValue(CanvasHeightProperty, value);
    }

    public static readonly StyledProperty<string> LayoutBackgroundPathProperty =
        AvaloniaProperty.Register<DesignLayoutControl, string>(nameof(LayoutBackgroundPath));

    public string LayoutBackgroundPath
    {
        get => this.GetValue(LayoutBackgroundPathProperty);
        set => SetValue(LayoutBackgroundPathProperty, value);
    }

    public static readonly StyledProperty<double> ButtonWidthProperty =
        AvaloniaProperty.Register<DesignLayoutControl, double>(nameof(ButtonWidth));

    public double ButtonWidth
    {
        get => this.GetValue(ButtonWidthProperty);
        set => SetValue(ButtonWidthProperty, value);
    }

    public static readonly StyledProperty<double> ButtonHeightProperty =
        AvaloniaProperty.Register<DesignLayoutControl, double>(nameof(ButtonHeight));

    public double ButtonHeight
    {
        get => this.GetValue(ButtonHeightProperty);
        set => SetValue(ButtonHeightProperty, value);
    }

    public static readonly StyledProperty<bool> ActiveLayerSwitchProperty =
        AvaloniaProperty.Register<DesignLayoutControl, bool>(nameof(ActiveLayerSwitch), true);

    public bool ActiveLayerSwitch
    {
        get => this.GetValue(ActiveLayerSwitchProperty);
        set => SetValue(ActiveLayerSwitchProperty, value);
    }

    public static readonly StyledProperty<double> SizeRatioProperty =
        AvaloniaProperty.Register<DesignLayoutControl, double>(nameof(SizeRatio), 1);

    public double SizeRatio
    {
        get => this.GetValue(SizeRatioProperty);
        set => SetValue(SizeRatioProperty, value);
    }

    public static readonly StyledProperty<ICommand> SwitchLayerProperty =
        AvaloniaProperty.Register<DesignLayoutControl, ICommand>(nameof(SwitchLayerCommand));

    public ICommand SwitchLayerCommand
    {
        get => this.GetValue(SwitchLayerProperty);
        set => SetValue(SwitchLayerProperty, value);
    }

    public static readonly StyledProperty<ICommand> NextBackgroundProperty =
        AvaloniaProperty.Register<DesignLayoutControl, ICommand>(nameof(NextBackground));

    public ICommand NextBackground
    {
        get => this.GetValue(NextBackgroundProperty);
        set => SetValue(NextBackgroundProperty, value);
    }

    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<DesignLayoutControl, object>(nameof(CommandParameter));

    public object CommandParameter
    {
        get => this.GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly StyledProperty<ICommand> AddPhotoCommandProperty =
        AvaloniaProperty.Register<DesignLayoutControl, ICommand>(nameof(AddPhotoCommand));

    public ICommand AddPhotoCommand
    {
        get => this.GetValue(AddPhotoCommandProperty);
        set => SetValue(AddPhotoCommandProperty, value);
    }

    public static readonly StyledProperty<IList<LayoutImageEntity>> PhotoImagesProperty =
        AvaloniaProperty.Register<DesignLayoutControl, IList<LayoutImageEntity>>(nameof(PhotoImages));

    public static readonly StyledProperty<ICommand> RemovePhotoCommandProperty =
        AvaloniaProperty.Register<DesignLayoutControl, ICommand>(nameof(RemovePhotoCommand));

    public ICommand RemovePhotoCommand
    {
        get => this.GetValue(RemovePhotoCommandProperty);
        set => SetValue(RemovePhotoCommandProperty, value);
    }

    public IList<LayoutImageEntity> PhotoImages
    {
        get => this.GetValue(PhotoImagesProperty);
        set => SetValue(PhotoImagesProperty, value);
    }

    public static readonly StyledProperty<IList<OverlayLayoutImage>> OverlayImagesProperty =
        AvaloniaProperty.Register<DesignLayoutControl, IList<OverlayLayoutImage>>(nameof(OverlayImages));

    public IList<OverlayLayoutImage> OverlayImages
    {
        get => this.GetValue(OverlayImagesProperty);
        set => SetValue(OverlayImagesProperty, value);
    }

    public DesignLayoutControl()
    {
        InitializeComponent();
        SwitchLayerCommand = new RelayCommand<bool>(SwitchLayers);
        AddPhotoCommand = new RelayCommand(AddPhoto);
        RemovePhotoCommand = new RelayCommand(RemovePhoto);
        canvasRoot.SizeChanged += PhotoCanvas_SizeChanged;
        this.GetObservable(LayoutBackgroundPathProperty).Subscribe(path =>
        {
            LoadBackgroundImage(path);
        });
    }

    private void PhotoCanvas_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        SetHeight(e.NewSize.Width);
    }

    private void Loaded(object? sender, RoutedEventArgs e)
    {
        var width = canvasRoot.Bounds.Width;
        SetHeight(width);
    }

    private void SetHeight(double width)
    {
        var height = width / SizeRatio;
        foreach (var child in canvasRoot.Children)
        {
            child.Width = width;
            child.Height = height;
        }
    }

    private void SwitchLayers(bool state)
    {
        ActiveLayerSwitch = state;
    }

    private void LoadBackgroundImage(string path)
    {
        if (String.IsNullOrEmpty(path) || !File.Exists(path))
        {
            canvasBackground.Source = null;
            return;
        }
        canvasBackground.Source = new Bitmap(path);
    }

    private void AddPhoto()
    {
        var index = (photoCanvas.Children.Count + 1).ToString();
        var image = new Grid()
        {
            Tag = index,
            Width = Consts.Sizes.Width * StartScale * (photoCanvas.Width / Consts.Sizes.BasicPrintWidth),
            Height = Consts.Sizes.Height * StartScale * (photoCanvas.Width / Consts.Sizes.BasicPrintWidth),
            Background = RandomColor(),
        };
        var indexText = new TextBlock()
        {
            Text = index.ToString(),
            FontSize = 30,
            Foreground = Brushes.White,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        SetTransformWithStartAngle(indexText, 270);
        SetTransformWithStartAngle(image, 90);
        Canvas.SetTop(image, 50);
        Canvas.SetLeft(image, 50);
        image.Children.Add(indexText);
        photoCanvas.Children.Add(image);
    }

    private void RemovePhoto()
    {
        if (photoCanvas.Children.Count > 0)
        {
            photoCanvas.Children.Remove(photoCanvas.Children[^1]);
        }
    }

    private void SetTransformWithStartAngle(Control control, double angle)
    {
        control.RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative);
        control.RenderTransform = new RotateTransform(angle);
    }

    private SolidColorBrush RandomColor()
    {
        Random rand = new Random();
        return new SolidColorBrush(Color.FromRgb((byte)rand.Next(50, 255), (byte)rand.Next(50, 255), (byte)rand.Next(50, 255)));
    }
}