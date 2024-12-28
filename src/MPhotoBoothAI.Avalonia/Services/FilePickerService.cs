using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AvaloniaApplication = Avalonia.Application;

namespace MPhotoBoothAI.Avalonia.Services;

public class FilePickerService : IFilePickerService
{
    private static IDictionary<FileTypes, FilePickerFileType> _pickerFileTypes = new Dictionary<FileTypes, FilePickerFileType>()
    {
        {FileTypes.All, FilePickerFileTypes.All},
        {FileTypes.AllImages, FilePickerFileTypes.ImageAll},
        {FileTypes.NonTransparentImages,
            new FilePickerFileType(string.Empty) {
                Patterns =[ "*.jpg", "*.jpeg", "*.bmp" ],
                AppleUniformTypeIdentifiers = new[] { "public.image" },
                MimeTypes = ["image/*" ]
            }
        },
        {FileTypes.OnlyTransparentImages,
            new FilePickerFileType(string.Empty) {
                Patterns = [ "*.png", "*.gif", "*.webp" ],
                AppleUniformTypeIdentifiers = ["public.image"],
                MimeTypes = ["image/*"]
            }
        },
        {FileTypes.Text,  FilePickerFileTypes.TextPlain}
    };

    public async Task<byte[]> PickFile(FileTypes fileTypes)
    {
        var file = await PickFileInternal(fileTypes);
        if (file != null)
        {
            await using var readStream = await file.OpenReadAsync();
            using var ms = new MemoryStream();
            await readStream.CopyToAsync(ms);
            return ms.ToArray();
        }
        return [];
    }

    public async Task<string> PickFilePath(FileTypes fileTypes)
    {
        var file = await PickFileInternal(fileTypes);
        if (file != null)
        {
            return file.Path.AbsolutePath;
        }
        return string.Empty;
    }

    private static async Task<IStorageFile?> PickFileInternal(FileTypes fileTypes)
    {
        if (AvaloniaApplication.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop || desktop.MainWindow?.StorageProvider is not { } provider)
        {
            throw new NullReferenceException("Missing StorageProvider instance.");
        }
        List<FilePickerFileType> filters = [];
        if (_pickerFileTypes.TryGetValue(fileTypes, out FilePickerFileType? filter) && filter != null)
        {
            filters.Add(filter);
        }
        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open File",
            AllowMultiple = false,
            FileTypeFilter = filters
        });
        return files?.Count >= 1 ? files[0] : null;
    }
}
