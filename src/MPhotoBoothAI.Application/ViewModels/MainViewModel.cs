using CommunityToolkit.Mvvm.ComponentModel;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.UI;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _content = default!;

    [ObservableProperty]
    private ListItemTemplate? _selectedPage;

    private readonly INavigationService<ViewModelBase> _navigationService;

    public ObservableCollection<ListItemTemplate> Pages { get; }

    private readonly List<ListItemTemplate> _pages =
    [
        new ListItemTemplate(typeof(HomeViewModel), "home_regular", "Home"),
        new ListItemTemplate(typeof(FaceDetectionViewModel), "person_swap_regular", "FaceDetection")
    ];

    public MainViewModel(INavigationService<ViewModelBase> navigationService)
    {
        Pages = new ObservableCollection<ListItemTemplate>(_pages);
        _navigationService = navigationService;
        _navigationService.CurrentViewModelChanged += viewModel => Content = viewModel;
        SelectedPage = Pages[0];
    }

    partial void OnSelectedPageChanged(ListItemTemplate? value)
    {
        if (value is null)
        {
            return;
        }
        _navigationService.GoTo(value.ModelType);
    }
}
