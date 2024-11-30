using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

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

    public DesignLayoutControl()
    {
        InitializeComponent();
    }

    private void LoadNextBackground(object? sender, RoutedEventArgs e)
    {
    }
}