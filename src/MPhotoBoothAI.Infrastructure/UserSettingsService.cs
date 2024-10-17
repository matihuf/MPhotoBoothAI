using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models;
using System.ComponentModel;
using System.Text.Json;

namespace MPhotoBoothAI.Infrastructure;
public class UserSettingsService : IUserSettingsService
{
    private readonly IApplicationInfoService _applicationInfoService;
    private static string _file => "settings.json";

    public UserSettings Value { get; }

    public UserSettingsService(IApplicationInfoService applicationInfoService)
    {
        _applicationInfoService = applicationInfoService;
        Value = Load();
        Value.PropertyChanged += UserSettings_PropertyChanged;
    }

    private void UserSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e) => Save();

    private UserSettings Load()
    {
        var path = _applicationInfoService.UserProfilePath;
        var fullPath = Path.Combine(path, _file);
        if (!Directory.Exists(path) || !File.Exists(fullPath))
        {
            return new UserSettings();
        }
        var userSettings = JsonSerializer.Deserialize<UserSettings>(File.ReadAllText(fullPath));
        return userSettings ?? new UserSettings();
    }

    private void Save()
    {
        var path = _applicationInfoService.UserProfilePath;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(Path.Combine(path, _file), JsonSerializer.Serialize(Value));
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Value.PropertyChanged -= UserSettings_PropertyChanged;
        }
    }
}
