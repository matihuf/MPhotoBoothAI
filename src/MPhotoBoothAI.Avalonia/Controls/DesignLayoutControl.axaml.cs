using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Key = Avalonia.Input.Key;

namespace MPhotoBoothAI.Avalonia.Controls;

public partial class DesignLayoutControl : UserControl
{
    private const double StartScale = 0.25;

    private const double StartAngle = 0;

    private Random _rand = new Random();

    private bool _isDraged;

    private Point _originTranslateTransform;

    private Point _startDragPosition;

    private bool _resizeModifier;

    private bool _resizeMultiplier;

    private double _scaleDividor = 5000;

    private readonly Point _startPivot = new Point(0, 0);

    private string[] _imageExtensions = [".tif", ".tiff", ".bmp", ".jpg", ".jpeg", ".png"];

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

    private void ModifierPressed(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.LeftCtrl)
        {
            _resizeModifier = true;
        }
        if (e.Key == Key.LeftShift)
        {
            _resizeMultiplier = true;
        }
    }

    private void ModifierReleased(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.LeftCtrl)
        {
            _resizeModifier = false;
        }
        if (e.Key == Key.LeftShift)
        {
            _resizeMultiplier = false;
        }
    }

    private void PhotoCanvas_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        SetHeight(e.NewSize.Width);
    }

    private void LoadedControl(object? sender, RoutedEventArgs e)
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

    private void Drop(object? sender, DragEventArgs e)
    {
        foreach (var file in e.Data.GetFiles())
        {
            var ratio = photoCanvas.Width / Consts.Sizes.BasicPrintWidth;
            var path = file.Path.LocalPath;
            var point = e.GetPosition(this);
            var extension = Path.GetExtension(path).ToLower();
            if (_imageExtensions.Contains(extension))
            {
                var bitmap = new Bitmap(path);
                var image = new Image()
                {
                    Source = bitmap,
                    Width = bitmap.Size.Width * ratio,
                    Height = bitmap.Size.Height * ratio,
                    RenderTransform = new RotateTransform(StartAngle, _startPivot.X, _startPivot.Y),
                };
                var context = new ContextMenu()
                {
                    Background = Brushes.DarkGray,
                    BorderBrush = Brushes.Transparent,
                    Width = 200,
                    Height = 200,
                };
                var items = new List<MenuItem>
                {
                    new MenuItem { Header = "Usuñ", Foreground= Brushes.White}
                };

                context.ItemsSource = items;
                image.ContextMenu = context;

                CreateItemOnLayer(image, frameCanvas, point);
            }
        }
    }

    private void AddPhoto()
    {
        var gridRect = GetImageGrid();
        CreateItemOnLayer(gridRect, photoCanvas, new Point(gridRect.Height / 2, gridRect.Width / 2), true);
    }

    private void CreateItemOnLayer(Control control, Canvas canvas, Point position, bool addIndex = false)
    {
        var index = (canvas.Children.Count + 1).ToString();
        var imageRoot = new Grid() { Width = 1, Height = 1, RenderTransform = new TranslateTransform() };
        Canvas.SetTop(imageRoot, position.Y);
        Canvas.SetLeft(imageRoot, position.X);
        RegisterEvents(control);
        if (control is Grid grid && addIndex)
        {
            var indexText = GetIndexText(index);
            grid.Children.Add(indexText);
        }
        imageRoot.Children.Add(control);
        canvas.Children.Add(imageRoot);
    }

    private Grid GetImageGrid()
    {
        return new Grid()
        {
            Width = Consts.Sizes.Height * StartScale * (photoCanvas.Width / Consts.Sizes.BasicPrintWidth),
            Height = Consts.Sizes.Width * StartScale * (photoCanvas.Width / Consts.Sizes.BasicPrintWidth),
            RenderTransform = new RotateTransform(StartAngle, _startPivot.X, _startPivot.Y),
            Background = RandomColor(),
        };
    }

    private static TextBlock GetIndexText(string index)
    {
        return new TextBlock()
        {
            Text = index.ToString(),
            FontSize = 30,
            Foreground = Brushes.White,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
    }

    private void RemovePhoto()
    {
        if (photoCanvas.Children.Count > 0)
        {
            var lastChild = photoCanvas.Children[^1] as Grid;
            UnregisterEvents(lastChild.Children[0]);
            photoCanvas.Children.Remove(photoCanvas.Children[^1]);
        }
    }

    private void RegisterEvents(Control control)
    {
        control.PointerPressed += StartMoveControl;
        control.PointerMoved += MoveControl;
        control.PointerReleased += EndMoveControl;
        control.PointerWheelChanged += RotateResizeControl;
    }

    private void UnregisterEvents(Control control)
    {
        control.PointerPressed -= StartMoveControl;
        control.PointerMoved -= MoveControl;
        control.PointerReleased -= EndMoveControl;
        control.PointerWheelChanged -= RotateResizeControl;
    }

    private void StartMoveControl(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Control control
            && control.Parent is Control parentControl
            && parentControl.RenderTransform is TranslateTransform translate)
        {
            _startDragPosition = e.GetPosition(this);
            _originTranslateTransform = new Point(translate.X, translate.Y);
            _isDraged = true;
        }
    }

    private void MoveControl(object? sender, PointerEventArgs e)
    {
        if (_isDraged && sender is Control control
            && control.Parent is Control parentControl
            && parentControl.RenderTransform is TranslateTransform translate)
        {
            var currentPosition = e.GetPosition(this);
            translate.X = _originTranslateTransform.X + (currentPosition.X - _startDragPosition.X);
            translate.Y = _originTranslateTransform.Y + (currentPosition.Y - _startDragPosition.Y);
        }
    }

    private void EndMoveControl(object? sender, PointerReleasedEventArgs e)
    {
        _isDraged = false;
    }

    private void RotateResizeControl(object? sender, PointerWheelEventArgs e)
    {
        if (sender is Control control)
        {
            var delta = e.Delta.Y;
            if (_resizeModifier)
            {
                double width = Consts.Sizes.Height;
                double height = Consts.Sizes.Width;
                if (control is Image image)
                {
                    width = image.Source.Size.Width;
                    height = image.Source.Size.Height;
                }
                var canvasToOriginalSizeRatio = photoCanvas.Width / Consts.Sizes.BasicPrintWidth;
                var currentScale = control.Width / (width * canvasToOriginalSizeRatio);
                var newScale = (currentScale - (delta / _scaleDividor * (_resizeMultiplier ? 5 : 1)));
                if (newScale > 0)
                {
                    control.Width = width * canvasToOriginalSizeRatio * newScale;
                    control.Height = height * canvasToOriginalSizeRatio * newScale;
                }
            }
            else
            {
                if (control.RenderTransform is RotateTransform rotate)
                {
                    var angle = rotate.Angle;
                    if (delta >= 0)
                    {
                        angle--;
                    }
                    else
                    {
                        angle++;
                    }
                    rotate.Angle = angle % 360;
                }
            }
            e.Handled = true;
        }
    }

    private SolidColorBrush RandomColor()
    {
        return new SolidColorBrush(Color.FromRgb((byte)_rand.Next(50, 255), (byte)_rand.Next(50, 255), (byte)_rand.Next(50, 255)));
    }
}