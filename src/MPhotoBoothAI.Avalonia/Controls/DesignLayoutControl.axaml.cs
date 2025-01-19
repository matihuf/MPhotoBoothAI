using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    private const double ResizeScaleDividor = 5000;

    private bool _isFormatLayoutChanged;

    private bool _isFormatDataChanged;

    private double _mainScale = 1;

    private Random _rand = new Random();

    private bool _isDraged;

    private Point _startDragOffset;

    private bool _resizeModifier;

    private bool _resizeMultiplier;

    private readonly Point _startPivot = new Point(0, 0);

    private string[] _imageExtensions = [".tif", ".tiff", ".bmp", ".jpg", ".jpeg", ".png"];

    private List<string> _overlayImagesPaths = [];

    public static readonly StyledProperty<string> LayoutBackgroundPathProperty =
        AvaloniaProperty.Register<DesignLayoutControl, string>(nameof(LayoutBackgroundPath));

    public string LayoutBackgroundPath
    {
        get => this.GetValue(LayoutBackgroundPathProperty);
        set => SetValue(LayoutBackgroundPathProperty, value);
    }

    public static readonly StyledProperty<bool> ActiveLayerSwitchProperty =
        AvaloniaProperty.Register<DesignLayoutControl, bool>(nameof(ActiveLayerSwitch), true);

    public bool ActiveLayerSwitch
    {
        get => this.GetValue(ActiveLayerSwitchProperty);
        set => SetValue(ActiveLayerSwitchProperty, value);
    }

    public static readonly StyledProperty<ICommand> NextBackgroundProperty =
        AvaloniaProperty.Register<DesignLayoutControl, ICommand>(nameof(NextBackground));

    public ICommand NextBackground
    {
        get => this.GetValue(NextBackgroundProperty);
        set => SetValue(NextBackgroundProperty, value);
    }

    public static readonly StyledProperty<ICommand> SaveLayoutCommandProperty =
        AvaloniaProperty.Register<DesignLayoutControl, ICommand>(nameof(SaveLayoutCommand));

    public ICommand SaveLayoutCommand
    {
        get => this.GetValue(SaveLayoutCommandProperty);
        set => SetValue(SaveLayoutCommandProperty, value);
    }

    public static readonly StyledProperty<IFilePickerService> FilePickerProperty =
        AvaloniaProperty.Register<DesignLayoutControl, IFilePickerService>(nameof(FilePicker));

    public IFilePickerService FilePicker
    {
        get => this.GetValue(FilePickerProperty);
        set => SetValue(FilePickerProperty, value);
    }

    public static readonly StyledProperty<LayoutFormatEntity> LayoutFormatProperty =
        AvaloniaProperty.Register<DesignLayoutControl, LayoutFormatEntity>(nameof(LayoutFormat));

    public LayoutFormatEntity LayoutFormat
    {
        get => this.GetValue(LayoutFormatProperty);
        set => SetValue(LayoutFormatProperty, value);
    }

    public static readonly StyledProperty<LayoutDataEntity> LayoutDataProperty =
        AvaloniaProperty.Register<DesignLayoutControl, LayoutDataEntity>(nameof(LayoutData));

    public LayoutDataEntity LayoutData
    {
        get => this.GetValue(LayoutDataProperty);
        set => SetValue(LayoutDataProperty, value);
    }

    public static readonly StyledProperty<bool> NotSavedChangeProperty =
        AvaloniaProperty.Register<DesignLayoutControl, bool>(nameof(NotSavedChange), defaultBindingMode: BindingMode.TwoWay);

    public bool NotSavedChange
    {
        get => this.GetValue(NotSavedChangeProperty);
        set => SetValue(NotSavedChangeProperty, value);
    }

    public DesignLayoutControl()
    {
        InitializeComponent();
        DataContext = this;
        canvasRoot.SizeChanged += PhotoCanvas_SizeChanged;
        this.GetObservable(LayoutBackgroundPathProperty).Subscribe(path =>
        {
            LoadBackgroundImage(path);
        });
        this.GetObservable(LayoutDataProperty).Subscribe(path =>
        {
            if (path != null)
            {
                _isFormatDataChanged = true;
                if (_isFormatLayoutChanged)
                {
                    ChangeLayout();
                }
            }
        });
        this.GetObservable(LayoutFormatProperty).Subscribe(path =>
        {
            if (path != null)
            {
                _isFormatLayoutChanged = true;
                if (_isFormatDataChanged)
                {
                    ChangeLayout();
                }
            }
        });
    }

    ~DesignLayoutControl()
    {
        canvasRoot.SizeChanged -= PhotoCanvas_SizeChanged;
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

    private void ChangeLayout()
    {
        _isFormatDataChanged = false;
        _isFormatLayoutChanged = false;
        var width = canvasRoot.Bounds.Width;
        if (width > 0 && LayoutData != null && LayoutFormat != null)
        {
            _mainScale = width / LayoutFormat.FormatWidth;
            SetHeight(width);
            Clear();
            LoadLayerItems();
        }
    }

    private void LoadedControl(object? sender, RoutedEventArgs e)
    {
        if (LayoutFormat is not null && LayoutData is not null)
        {
            ChangeLayout();
        }
    }

    private void LoadLayerItems()
    {
        foreach (var photo in LayoutData.PhotoLayoutData)
        {
            var photoGrid = BuildPhotoImageGrid();
            AddImageOnCanvas(photoGrid, photoCanvas, new Point(photo.Left * _mainScale, photo.Top * _mainScale), photo.Angle, photo.Scale / _mainScale, true);
        }
        foreach (var overlayImage in LayoutData.OverlayImageData)
        {
            if (File.Exists(overlayImage.Path))
            {
                _overlayImagesPaths.Add(overlayImage.Path);
                var image = BuildImage(new Bitmap(overlayImage.Path));
                AddImageOnCanvas(image, frameCanvas, new Point(overlayImage.Left * _mainScale, overlayImage.Top * _mainScale), overlayImage.Angle, overlayImage.Scale / _mainScale);
            }
        }
    }

    private void PhotoCanvas_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (LayoutFormat != null)
        {
            var width = e.NewSize.Width;
            var ratio = width / e.PreviousSize.Width;
            _mainScale = width / LayoutFormat.FormatWidth;
            SetHeight(width);
            ResizeReposiztionChildren(ratio, photoCanvas);
            ResizeReposiztionChildren(ratio, frameCanvas);
        }
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

    private void SetHeight(double width)
    {
        var height = width * LayoutFormat.FormatRatio;
        foreach (var child in canvasRoot.Children)
        {
            child.Width = width;
            child.Height = height;
        }
    }

    private void SwitchToPhotoLayer(object? sender, RoutedEventArgs e)
    {
        ActiveLayerSwitch = true;
    }

    private void SwitchToFrameLayer(object? sender, RoutedEventArgs e)
    {
        ActiveLayerSwitch = false;
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

    private async void AddFrame(object? sender, RoutedEventArgs e)
    {
        try
        {
            var file = await FilePicker.PickFilePath(FileTypes.AllImages);
            LoadImageFromPath(new Point(50, 50), file, _mainScale);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private void Drop(object? sender, DragEventArgs e)
    {
        var files = e.Data.GetFiles();
        if (files != null)
        {
            foreach (var file in files)
            {
                if (File.Exists(file.Path.LocalPath))
                {
                    LoadImageFromPath(e.GetPosition(this), file.Path.LocalPath, _mainScale);
                }
            }
        }
    }

    private void LoadImageFromPath(Point position, string path, double scale)
    {
        var extension = Path.GetExtension(path).ToLower();
        if (_imageExtensions.Contains(extension))
        {
            var bitmap = new Bitmap(path);
            _overlayImagesPaths.Add(path);
            Image image = BuildImage(bitmap);
            NotSavedChange = true;
            AddImageOnCanvas(image, frameCanvas, position, StartAngle, scale);
        }
    }

    private void AddPhoto(object? sender, RoutedEventArgs e)
    {
        var gridRect = BuildPhotoImageGrid();
        var position = new Point(gridRect.Width / 2 * _mainScale * StartScale, photoCanvas.Height / 2);
        NotSavedChange = true;
        AddImageOnCanvas(gridRect, photoCanvas, position, 0, _mainScale * StartScale, true);
    }

    private void AddImageOnCanvas(Control control, Canvas canvas, Point position, double angle, double scale, bool addIndex = false)
    {
        var index = (canvas.Children.Count + 1).ToString();
        var imageRoot = new Grid() { Width = 0, Height = 0 };
        control.RenderTransform = new RotateTransform(angle, _startPivot.X, _startPivot.Y);
        control.Width *= scale;
        control.Height *= scale;
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

    private Grid BuildPhotoImageGrid()
    {
        return new Grid()
        {
            Width = Consts.Sizes.PhotoHeight,
            Height = Consts.Sizes.PhotoWidth,
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

    private Image BuildImage(Bitmap bitmap)
    {
        return new Image()
        {
            Source = bitmap,
            Width = bitmap.Size.Width,
            Height = bitmap.Size.Height,
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
                    var index = frameCanvas.Children.IndexOf(rootGrid);
                    _overlayImagesPaths.RemoveAt(index);
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
                var cloneImage = BuildImage(bitmap);
                cloneImage.Width = image.Width;
                cloneImage.Height = image.Height;
                Point position = new(Canvas.GetLeft(rootGrid) + 50, Canvas.GetTop(rootGrid) + 50);
                var pathToCopy = frameCanvas.Children.IndexOf(rootGrid);
                _overlayImagesPaths.Add(_overlayImagesPaths[pathToCopy]);
                AddImageOnCanvas(cloneImage, frameCanvas, position, StartAngle, 1);
            }
        }
    }

    private void RemovePhoto(object? sender, RoutedEventArgs e)
    {
        if (photoCanvas.Children.Count > 0 && photoCanvas.Children[^1] is Grid item)
        {
            RemoveItemFromLayer(item, photoCanvas);
            NotSavedChange = true;
        }
    }

    private void RemoveItemFromLayer(Control item, Canvas layer)
    {
        if (layer.Children.Contains(item))
        {
            UnregisterEvents(item);
            layer.Children.Remove(item);
            NotSavedChange = true;
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
            NotSavedChange = true;
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
                var currentScale = control.Width / (width * _mainScale);
                var newScale = (currentScale - (delta / ResizeScaleDividor * (_resizeMultiplier ? 5 : 1)));
                if (newScale > 0)
                {
                    control.Width = width * _mainScale * newScale;
                    control.Height = height * _mainScale * newScale;
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
            NotSavedChange = true;
        }
    }

    private SolidColorBrush RandomColor()
    {
        return new SolidColorBrush(Color.FromRgb((byte)_rand.Next(50, 255), (byte)_rand.Next(50, 255), (byte)_rand.Next(50, 255)));
    }

    private void SaveLayout(object? sender, RoutedEventArgs e)
    {
        LayoutData.PhotoLayoutData.Clear();
        LayoutData.OverlayImageData.Clear();
        for (int i = 0; i < photoCanvas.Children.Count; i++)
        {
            Grid grid = (Grid)photoCanvas.Children[i];
            var photo = new PhotoLayoutDataEntity
            {
                Top = Canvas.GetTop(grid) / _mainScale,
                Left = Canvas.GetLeft(grid) / _mainScale,
                Angle = (grid.Children[0].RenderTransform as RotateTransform).Angle,
                Scale = grid.Children[0].Height / Consts.Sizes.PhotoWidth * _mainScale,
            };
            LayoutData.PhotoLayoutData.Add(photo);
        }
        for (int i = 0; i < frameCanvas.Children.Count; i++)
        {
            Grid grid = (Grid)frameCanvas.Children[i];
            var image = grid.Children[0] as Image;
            var bitmap = image.Source as Bitmap;
            var overlayImage = new OverlayImageDataEntity
            {
                Top = Canvas.GetTop(grid) / _mainScale,
                Left = Canvas.GetLeft(grid) / _mainScale,
                Angle = (image.RenderTransform as RotateTransform).Angle,
                Scale = image.Width / bitmap.Size.Width * _mainScale,
                Path = _overlayImagesPaths[frameCanvas.Children.IndexOf(grid)]
            };
            LayoutData.OverlayImageData.Add(overlayImage);
        }
        SaveLayoutCommand?.Execute(null);
    }

    private void ClearLayout(object? sender, RoutedEventArgs e)
    {
        Clear();
        NotSavedChange = true;
    }

    private void Clear()
    {
        photoCanvas.Children.Clear();
        frameCanvas.Children.Clear();
    }
}