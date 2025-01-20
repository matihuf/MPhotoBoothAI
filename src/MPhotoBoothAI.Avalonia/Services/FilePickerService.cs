using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MPhotoBoothAI.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AvaloniaApplication = Avalonia.Application;

namespace MPhotoBoothAI.Avalonia.Services;

public class FilePickerService : IFilePickerService
{
    private readonly Dictionary<MPhotoBoothAI.Models.Enums.FilePickerFileType, FilePickerFileType> _fileTypes = new()
    {
        { MPhotoBoothAI.Models.Enums.FilePickerFileType.Pdf, FilePickerFileTypes.Pdf  },
        { MPhotoBoothAI.Models.Enums.FilePickerFileType.Text, FilePickerFileTypes.TextPlain  },
        { MPhotoBoothAI.Models.Enums.FilePickerFileType.Image, ImageAll  }
    };

    public async Task<byte[]> PickFile(MPhotoBoothAI.Models.Enums.FilePickerFileType[]? filePickerFileTypes = null)
    {
        var file = await PickFileInternal(filePickerFileTypes);
        if (file != null)
        {
            await using var readStream = await file.OpenReadAsync();
            using var ms = new MemoryStream();
            await readStream.CopyToAsync(ms);
            return ms.ToArray();
        }
        return [];
    }

    public async Task<string> PickFilePath(MPhotoBoothAI.Models.Enums.FilePickerFileType[]? filePickerFileTypes = null)
    {
        var file = await PickFileInternal(filePickerFileTypes);
        if (file != null)
        {
            return file.Path.AbsolutePath;
        }
        return string.Empty;
    }

    private async Task<IStorageFile?> PickFileInternal(MPhotoBoothAI.Models.Enums.FilePickerFileType[]? filePickerFileTypes)
    {
        if (AvaloniaApplication.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop || desktop.MainWindow?.StorageProvider is not { } provider)
        {
            throw new NullReferenceException("Missing StorageProvider instance.");
        }

        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open File",
            AllowMultiple = false,
            FileTypeFilter = Map(filePickerFileTypes)
        });
        return files?.Count >= 1 ? files[0] : null;
    }

    private FilePickerFileType[]? Map(MPhotoBoothAI.Models.Enums.FilePickerFileType[]? filePickerFileTypes)
    {
        if (filePickerFileTypes == null)
        {
            return null;
        }
        var types = new FilePickerFileType[filePickerFileTypes.Length];
        for (int i = 0; i < filePickerFileTypes.Length; i++)
        {
            types[i] = _fileTypes[filePickerFileTypes[i]];
        }
        return types;
    }

    private static FilePickerFileType ImageAll { get; } = new("All Images")
    {
        Patterns = ["*.png", "*.jpg", "*.jpeg"],
        AppleUniformTypeIdentifiers = ["public.image"],
        MimeTypes = ["image/*"]
    };
}
