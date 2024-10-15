using MPhotoBoothAI.Application.Interfaces;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace MPhotoBoothAI.Infrastructure;
public class UserSettings : IUserSettings
{
    private static UserSettings? _instance;
    public static UserSettings Instance => _instance ??= Load();

    private static string _file => "settings.json";

    private string _cultureInfoName = string.Empty;
    public string CultureInfoName
    {
        get => _cultureInfoName;
        set
        {
            _cultureInfoName = value;
            Save();
        }
    }

    private static UserSettings Load()
    {
        var path = GetPath();
        var fullPath = $"{path}/{_file}";
        if (!Directory.Exists(path) || !File.Exists(fullPath))
        {
            return new UserSettings();
        }
        var userSettings = JsonSerializer.Deserialize<UserSettings>(File.ReadAllText(fullPath));
        return userSettings ?? new UserSettings();
    }

    private static void Save()
    {
        var path = GetPath();
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText($"{path}/{_file}", JsonSerializer.Serialize(_instance));
    }

    private static string GetPath()
    {
        Assembly assembly = Assembly.GetEntryAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), fvi.CompanyName, fvi.ProductName);
    }
}
