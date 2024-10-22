using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Models.UI;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Avalonia.Controls;

public class CrudListBoxTemplatedControl : TemplatedControl
{
    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<CrudListBoxTemplatedControl, string>(nameof(Title));
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<IRelayCommand> AddCommandProperty = AvaloniaProperty.Register<CrudListBoxTemplatedControl, IRelayCommand>(nameof(AddCommand));
    public IRelayCommand AddCommand
    {
        get => GetValue(AddCommandProperty);
        set => SetValue(AddCommandProperty, value);
    }

    public static readonly StyledProperty<IRelayCommand<CrudListItem>> DeleteCommandProperty = AvaloniaProperty.Register<CrudListBoxTemplatedControl, IRelayCommand<CrudListItem>>(nameof(DeleteCommand));
    public IRelayCommand<CrudListItem> DeleteCommand
    {
        get => GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public static readonly StyledProperty<ObservableCollection<CrudListItem>> ItemsSourceProperty = AvaloniaProperty.Register<CrudListBoxTemplatedControl, ObservableCollection<CrudListItem>>(nameof(ItemsSource));
    public ObservableCollection<CrudListItem> ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly StyledProperty<CrudListItem?> SelectedItemProperty = AvaloniaProperty.Register<CrudListBoxTemplatedControl, CrudListItem?>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);
    public CrudListItem? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
}