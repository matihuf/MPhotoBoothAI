using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.ViewModels;
public partial class DesignPrintTemplateViewModel : ViewModelBase
{
    private readonly IFilePickerService _filePickerService;

    private readonly IFilesManager _filesManager;

    private readonly IApplicationInfoService _applicationInfoService;

    private readonly IImageManager _imageManager;

    private readonly IMessageBoxService _messageBoxService;

    private readonly Dictionary<FormatTypes, double> _ratios = [];

    private readonly Dictionary<FormatTypes, string> _backgroundDir = [];

    [ObservableProperty]
    private Dictionary<FormatTypes, List<LayoutImageEntity>> _photoImages = [];

    [ObservableProperty]
    private Dictionary<FormatTypes, List<OverlayLayoutImage>> _overlayImages = [];

    [ObservableProperty]
    private Dictionary<FormatTypes, BackgroundInfo> _formats = [];

    public DesignPrintTemplateViewModel(IFilePickerService filePickerService,
        IFilesManager filesManager,
        IApplicationInfoService applicationInfoService,
        IImageManager imageManager,
        IMessageBoxService messageBoxService)
    {
        _filePickerService = filePickerService;
        _filesManager = filesManager;
        _applicationInfoService = applicationInfoService;
        _imageManager = imageManager;
        _messageBoxService = messageBoxService;
        foreach (FormatTypes format in Enum.GetValues(typeof(FormatTypes)))
        {
            Formats.Add(format, new BackgroundInfo());
            _backgroundDir.Add(format, Path.Combine(_applicationInfoService.BackgroundDirectory, format.ToString()));
            PhotoImages.Add(format, []);
            OverlayImages.Add(format, []);
            PopulateBackgroundList(_backgroundDir[format], Formats[format]);
        }
        _ratios.Add(FormatTypes.Stripe, Consts.Background.StripeBackgroundRatio);
        _ratios.Add(FormatTypes.PostCard, Consts.Background.PostcardBackgroundRatio);
    }

    private void DeleteItem(BackgroundInfo backgroundInfo)
    {
        if (backgroundInfo.SelectedItem != null)
        {
            var selectedValueCopy = backgroundInfo.SelectedItem;
            _filesManager.DeleteFile(backgroundInfo.SelectedItem);
            backgroundInfo.BackgroundPathsList.Remove(backgroundInfo.SelectedItem);
            if (backgroundInfo.BackgroundPath == selectedValueCopy)
            {
                if (backgroundInfo.BackgroundPathsList.Count > 0)
                {
                    backgroundInfo.BackgroundPath = backgroundInfo.BackgroundPathsList[0];
                    return;
                }
                backgroundInfo.BackgroundPath = null;
            }
        }
    }

    private void PopulateBackgroundList(string pathToCopy, BackgroundInfo backgroundInfo)
    {
        backgroundInfo.BackgroundPathsList.Clear();
        foreach (var path in _filesManager.GetFiles(pathToCopy))
        {
            backgroundInfo.BackgroundPathsList.Add(path);
        }
        if (String.IsNullOrEmpty(backgroundInfo.BackgroundPath) && backgroundInfo.BackgroundPathsList.Count > 0)
        {
            backgroundInfo.BackgroundPath = backgroundInfo.BackgroundPathsList[0];
        }
    }

    private async Task AddBackgroundToList(FormatTypes format, IMainWindow mainWindow)
    {
        var pickFile = await _filePickerService.PickFilePath(Models.FileTypes.NonTransparentImages);
        var pathToCopy = _backgroundDir[format];
        var imageSize = _imageManager.GetImageSizeFromFile(pickFile);
        if (imageSize.HasValue)
        {
            var ratio = (double)imageSize.Value.Width / imageSize.Value.Height;
            if (ratio != _ratios[format] && !await _messageBoxService.ShowYesNo(Assets.UI.wrongImageRatioTitle, Assets.UI.wrongImageRatioMessage, mainWindow))
            {
                return;
            }
        }
        else
        {
            return;
        }
        _filesManager.CopyFile(pickFile, pathToCopy);
        PopulateBackgroundList(pathToCopy, Formats[format]);
    }

    [RelayCommand]
    private Task AddPostcardBackgroundToList(IMainWindow mainWindow)
    {
        return AddBackgroundToList(FormatTypes.PostCard, mainWindow);
    }

    [RelayCommand]
    private Task AddStripeBackgroundToList(IMainWindow mainWindow)
    {
        return AddBackgroundToList(FormatTypes.Stripe, mainWindow);
    }

    [RelayCommand]
    private void RemoveBackgroundFromList(FormatTypes format)
    {
        DeleteItem(Formats[format]);
    }

    [RelayCommand]
    private void LoadNextBackground(FormatTypes format)
    {
        LoadNextBackground(Formats[format]);
    }

    private void LoadNextBackground(BackgroundInfo backgroundInfo)
    {
        var count = backgroundInfo.BackgroundPathsList.Count;
        if (count > 0 && !String.IsNullOrEmpty(backgroundInfo.BackgroundPath))
        {
            var index = backgroundInfo.BackgroundPathsList.IndexOf(backgroundInfo.BackgroundPath);
            index++;
            if (index == count)
            {
                index = 0;
            }
            backgroundInfo.BackgroundPath = backgroundInfo.BackgroundPathsList[index];
        }
    }
}
