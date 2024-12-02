using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
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
    }

    private void SwitchLayers(bool state)
    {
        ActiveLayerSwitch = state;
    }
}