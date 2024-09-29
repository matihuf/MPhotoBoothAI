using CommunityToolkit.Mvvm.ComponentModel;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Navigation;

namespace MPhotoBoothAI.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _content = default!;

    public MainViewModel(HistoryRouter<ViewModelBase> router)
    {
        router.CurrentViewModelChanged += viewModel => Content = viewModel;
        router.GoTo<HomeViewModel>();
    }
}
