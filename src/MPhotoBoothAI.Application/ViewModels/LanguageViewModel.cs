using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
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

    private readonly IDatabaseContext _databaseContext;

    private readonly IAppRestarterService _appRestarterService;

    private readonly string _default;

    public LanguageViewModel(IDatabaseContext databaseContext, IAppRestarterService appRestarterService)
    {
        _databaseContext = databaseContext;
        _appRestarterService = appRestarterService;
        SelectedCultureInfo = Cultures.FirstOrDefault(x => x.Name == _databaseContext.UserSettings.AsNoTracking().FirstOrDefault()?.CultureInfoName) ?? Cultures.First();
        _default = SelectedCultureInfo.Name;
        IsRestartVisible = false;
    }

    partial void OnSelectedCultureInfoChanged(CultureInfo value)
    {
        _databaseContext.UserSettings.ExecuteUpdate(s => s.SetProperty(b => b.CultureInfoName, value.Name));
        IsRestartVisible = value.Name != _default;
    }

    [RelayCommand]
    public void RestartApplicationCommand() => _appRestarterService.Restart();
}