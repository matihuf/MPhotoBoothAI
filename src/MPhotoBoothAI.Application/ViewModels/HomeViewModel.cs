
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly Dictionary<string, Type> _navigations;

    public HomeViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        _navigations = new()
        {
            { "faceDetection", typeof(FaceDetectionViewModel) }
        };
    }

    [RelayCommand]
    private void Navigate(string parameter = "")
    {
        if (_navigations.TryGetValue(parameter, out Type? type))
        {
            _navigationService.GoTo(type);
        }
    }
}
