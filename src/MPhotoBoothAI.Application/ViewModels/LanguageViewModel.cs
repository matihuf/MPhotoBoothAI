using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Application.Interfaces;
using System.Globalization;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class LanguageViewModel : ViewModelBase
{
    public static IEnumerable<CultureInfo> Cultures =>
    [
        new CultureInfo("pl-PL"),
        new CultureInfo("en-US")
    ];

    [ObservableProperty]
    private CultureInfo _selectedCultureInfo;

    [ObservableProperty]
    private bool _isRestartVisible;

    private readonly IUserSettings _userSettings;
    private readonly IAppRestarterService _appRestarterService;
    private readonly string _default;

    public LanguageViewModel(IUserSettings userSettings, IAppRestarterService appRestarterService)
    {
        _userSettings = userSettings;
        _appRestarterService = appRestarterService;
        SelectedCultureInfo = Cultures.FirstOrDefault(x => x.Name == userSettings.CultureInfoName) ?? Cultures.First();
        _default = SelectedCultureInfo.Name;
        IsRestartVisible = false;
    }

    partial void OnSelectedCultureInfoChanged(CultureInfo value)
    {
        _userSettings.CultureInfoName = value.Name;
        IsRestartVisible = value.Name != _default;
    }

    [RelayCommand]
    public void RestartApplicationCommand() => _appRestarterService.Restart();
}