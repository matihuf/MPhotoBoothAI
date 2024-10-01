using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Navigation;
using MPhotoBoothAI.Models.UI;

namespace MPhotoBoothAI.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _content = default!;

    [ObservableProperty]
    private ListItemTemplate? _selectedPage;

    private readonly HistoryRouter<ViewModelBase> _router;

    public ObservableCollection<ListItemTemplate> Pages { get; }

    private readonly List<ListItemTemplate> _pages =
    [
        new ListItemTemplate(typeof(HomeViewModel), "home_regular", "Home"),
        new ListItemTemplate(typeof(FaceDetectionViewModel), "person_swap_regular", "FaceDetection")
    ];

    public MainViewModel(HistoryRouter<ViewModelBase> router)
    {
        Pages = new ObservableCollection<ListItemTemplate>(_pages);
        _router = router;
        _router.CurrentViewModelChanged += viewModel => Content = viewModel;
        SelectedPage = Pages[0];
    }

    partial void OnSelectedPageChanged(ListItemTemplate? value)
    {
        if (value is null)
        {
            return;
        }
        _router.GoTo(value.ModelType);
    }
}
