using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.WindowParameters;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class HomeViewModel(LanguageViewModel languageViewModel, IWindowService windowService) : ViewModelBase
{
    public LanguageViewModel Language => languageViewModel;

    [RelayCommand]
    private void Open(IMainWindow mainWindow) => windowService.Open(typeof(AddFaceSwapTemplateViewModel), mainWindow, new AddFaceSwapTemplateParameters { GroupName = "kaszanka" });
}
