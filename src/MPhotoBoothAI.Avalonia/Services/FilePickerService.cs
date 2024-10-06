using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MPhotoBoothAI.Application.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using AvaloniaApplication = Avalonia.Application;

namespace MPhotoBoothAI.Avalonia.Services;

public class FilePickerService : IFilePickerService
{
    public async Task<byte[]> PickFile()
    {
        var file = await PickFileInternal();
        if (file != null)
        {
            await using var readStream = await file.OpenReadAsync();
            using var ms = new MemoryStream();
            await readStream.CopyToAsync(ms);
            return ms.ToArray();
        }
        return [];
    }

    public async Task<string> PickFilePath()
    {
        var file = await PickFileInternal();
        if (file != null)
        {
            return file.Path.AbsolutePath;
        }
        return string.Empty;
    }

    private static async Task<IStorageFile?> PickFileInternal()
    {
        if (AvaloniaApplication.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop || desktop.MainWindow?.StorageProvider is not { } provider)
        {
            throw new NullReferenceException("Missing StorageProvider instance.");
        }

        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open File",
            AllowMultiple = false
        });
        return files?.Count >= 1 ? files[0] : null;
    }
}
