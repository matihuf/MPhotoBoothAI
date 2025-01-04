using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Avalonia.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Key = Avalonia.Input.Key;

namespace MPhotoBoothAI.Avalonia.Controls;

public partial class DesignLayoutControl : UserControl
{
    private const double StartScale = 0.25;

    private const double StartAngle = 0;

    private double _mainRatio = 1;

    private Random _rand = new Random();

    private bool _isDraged;

    private Point _startDragOffset;

    private bool _resizeModifier;

    private bool _resizeMultiplier;

    private double _scaleDividor = 5000;

    private readonly Point _startPivot = new Point(0, 0);

    private string[] _imageExtensions = [".tif", ".tiff", ".bmp", ".jpg", ".jpeg", ".png"];

    private IFilePickerService _filesPicker;

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

    public static readonly StyledProperty<ICommand> AddFrameCommandProperty =
        AvaloniaProperty.Register<DesignLayoutControl, ICommand>(nameof(AddFrameCommand));

    public ICommand AddFrameCommand
    {
        get => this.GetValue(AddFrameCommandProperty);
        set => SetValue(AddFrameCommandProperty, value);
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
        AddFrameCommand = new RelayCommand(async () => await AddFrame());
        canvasRoot.SizeChanged += PhotoCanvas_SizeChanged;
        this.GetObservable(LayoutBackgroundPathProperty).Subscribe(path =>
        {
            LoadBackgroundImage(path);
        });
        _filesPicker = new FilePickerService();
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
        var width = e.NewSize.Width;
        var ratio = width / e.PreviousSize.Width;
        _mainRatio = width / Consts.Sizes.BasicPrintWidth;
        SetHeight(width);
        ResizeReposiztionChildren(ratio, photoCanvas);
        ResizeReposiztionChildren(ratio, frameCanvas);
    }

    private void ResizeReposiztionChildren(double ratio, Canvas canvas)
    {
        foreach (Grid photoGrid in canvas.Children)
        {
            Control control = photoGrid.Children[0];
            control.Width *= ratio;
            control.Height *= ratio;
            Canvas.SetLeft(photoGrid, Canvas.GetLeft(photoGrid) * ratio);
            Canvas.SetTop(photoGrid, Canvas.GetTop(photoGrid) * ratio);
        }
    }

    private void LoadedControl(object? sender, RoutedEventArgs e)
    {
        var width = canvasRoot.Bounds.Width;
        _mainRatio = width / Consts.Sizes.BasicPrintWidth;
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

    private async Task AddFrame()
    {
        var file = await _filesPicker.PickFilePath(FileTypes.AllImages);
        LoadImageFromPath(new Point(50, 50), file);
    }

    private void Drop(object? sender, DragEventArgs e)
    {
        var files = e.Data.GetFiles();
        if (files != null)
        {
            foreach (var file in files)
            {
                LoadImageFromPath(e.GetPosition(this), file.Path.LocalPath);
            }
        }
    }

    private void LoadImageFromPath(Point position, string path)
    {
        var ratio = photoCanvas.Width / Consts.Sizes.BasicPrintWidth;
        var extension = Path.GetExtension(path).ToLower();
        if (_imageExtensions.Contains(extension))
        {
            var bitmap = new Bitmap(path);
            Image image = BuildImage(ratio, bitmap);
            AddItemOnLayer(image, frameCanvas, position);
        }
    }

    private void AddPhoto()
    {
        var gridRect = BuildImageGrid();
        AddItemOnLayer(gridRect, photoCanvas, new Point(gridRect.Height / 2, gridRect.Width / 2), true);
    }

    private void AddItemOnLayer(Control control, Canvas canvas, Point position, bool addIndex = false)
    {
        var index = (canvas.Children.Count + 1).ToString();
        var imageRoot = new Grid() { Width = 0, Height = 0 };
        Canvas.SetTop(imageRoot, position.Y);
        Canvas.SetLeft(imageRoot, position.X);
        RegisterEvents(control);
        if (control is Grid grid && addIndex)
        {
            var indexText = BuildIndexText(index);
            grid.Children.Add(indexText);
        }
        imageRoot.Children.Add(control);
        canvas.Children.Add(imageRoot);
    }

    private ContextMenu BuildContextMenu()
    {
        var clone = new MenuItem() { Header = Application.Assets.UI.clone };
        var remove = new MenuItem() { Header = Application.Assets.UI.remove };
        clone.Click += CloneItem;
        remove.Click += RemoveItem;
        return new ContextMenu()
        {
            ItemsSource = new List<MenuItem>
            {
                clone, remove
            }
        };
    }

    private Grid BuildImageGrid()
    {
        return new Grid()
        {
            Width = Consts.Sizes.PhotoHeight * StartScale * _mainRatio,
            Height = Consts.Sizes.PhotoWidth * StartScale * _mainRatio,
            RenderTransform = new RotateTransform(StartAngle, _startPivot.X, _startPivot.Y),
            Background = RandomColor()
        };
    }

    private static TextBlock BuildIndexText(string index)
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

    private Image BuildImage(double ratio, Bitmap bitmap)
    {
        return new Image()
        {
            Source = bitmap,
            Width = bitmap.Size.Width * ratio,
            Height = bitmap.Size.Height * ratio,
            RenderTransform = new RotateTransform(StartAngle, _startPivot.X, _startPivot.Y),
            ClipToBounds = false,
            ContextMenu = BuildContextMenu()
        };
    }

    private void RemoveItem(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            menuItem.Click -= RemoveItem;
            var contextMenu = menuItem.GetLogicalParent<ContextMenu>();
            if (contextMenu != null && contextMenu.Items[0] is MenuItem clone)
            {
                clone.Click -= CloneItem;
                if (contextMenu.Parent?.Parent?.Parent is Grid rootGrid)
                {
                    RemoveItemFromLayer(rootGrid, frameCanvas);
                }
            }
        }
    }

    private void CloneItem(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            var contextMenu = menuItem.GetLogicalParent<ContextMenu>();
            if (contextMenu != null && contextMenu.Parent?.Parent is Image image && image.Parent is Grid rootGrid && image.Source is Bitmap bitmap)
            {
                var ratio = image.Width / image.Source.Size.Width;
                var cloneImage = BuildImage(ratio, bitmap);
                Point position = new(Canvas.GetLeft(rootGrid) + 50, Canvas.GetTop(rootGrid) + 50);
                AddItemOnLayer(cloneImage, frameCanvas, position);
            }
        }
    }

    private void RemovePhoto()
    {
        if (photoCanvas.Children.Count > 0 && photoCanvas.Children[^1] is Grid item)
        {
            RemoveItemFromLayer(item, photoCanvas);
        }
    }

    private void RemoveItemFromLayer(Control item, Canvas layer)
    {
        if (layer.Children.Contains(item))
        {
            UnregisterEvents(item);
            layer.Children.Remove(item);
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
        if (e.GetCurrentPoint(null).Properties.PointerUpdateKind != PointerUpdateKind.RightButtonPressed
            && sender is Control control
            && control.Parent is Control parentControl)
        {
            var positionOnCanvas = e.GetPosition(photoCanvas);
            _startDragOffset = new Point(Canvas.GetLeft(parentControl) - positionOnCanvas.X, Canvas.GetTop(parentControl) - positionOnCanvas.Y);
            _isDraged = true;
        }
    }

    private void MoveControl(object? sender, PointerEventArgs e)
    {
        if (_isDraged && sender is Control control
            && control.Parent is Control parentControl)
        {
            var currentPosition = e.GetPosition(photoCanvas);
            Canvas.SetTop(parentControl, currentPosition.Y + _startDragOffset.Y);
            Canvas.SetLeft(parentControl, currentPosition.X + _startDragOffset.X);
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
                double width = Consts.Sizes.PhotoHeight;
                double height = Consts.Sizes.PhotoWidth;
                if (control is Image image && image.Source != null)
                {
                    width = image.Source.Size.Width;
                    height = image.Source.Size.Height;
                }
                var currentScale = control.Width / (width * _mainRatio);
                var newScale = (currentScale - (delta / _scaleDividor * (_resizeMultiplier ? 5 : 1)));
                if (newScale > 0)
                {
                    control.Width = width * _mainRatio * newScale;
                    control.Height = height * _mainRatio * newScale;
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