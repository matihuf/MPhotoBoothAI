namespace MPhotoBoothAI.Application.ViewModels;

public partial class HomeViewModel(LanguageViewModel languageViewModel) : ViewModelBase
{
    public LanguageViewModel Language => languageViewModel;
}
