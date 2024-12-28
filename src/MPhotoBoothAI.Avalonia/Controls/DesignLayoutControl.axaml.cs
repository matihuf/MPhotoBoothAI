using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;

namespace MPhotoBoothAI.Avalonia.Controls;

public partial class DesignLayoutControl : UserControl
{
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
        set
        {
            SetValue(LayoutBackgroundPathProperty, value);
            LoadBackgroundImage(value);
        }
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

    public DesignLayoutControl()
    {
        InitializeComponent();
        DataContext = this;
        SwitchLayerCommand = new RelayCommand<bool>(SwitchLayers);
        canvasRoot.SizeChanged += PhotoCanvas_SizeChanged;
        LayoutBackgroundPathProperty.Changed.Subscribe(e => LoadBackgroundImage(e.NewValue.Value));
        LoadBackgroundImage(LayoutBackgroundPath);
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
        }
    }
}
