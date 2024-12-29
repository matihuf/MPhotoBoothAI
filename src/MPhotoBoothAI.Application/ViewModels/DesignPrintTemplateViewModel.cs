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

    [ObservableProperty]
    private BackgroundInfo _stripeBackgroundInfo;

    [ObservableProperty]
    private BackgroundInfo _postcardBackgroundInfo;

    [ObservableProperty]
    private string? _postcardSelectedItem;

    [ObservableProperty]
    private string? _stripeSelectedItem;

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
        PostcardBackgroundInfo = new();
        StripeBackgroundInfo = new();
        PopulateBackgroundList(_applicationInfoService.PostcardBackgroundPath, PostcardBackgroundInfo);
        PopulateBackgroundList(_applicationInfoService.StripeBackgroundPath, StripeBackgroundInfo);
    }

    private async Task AddBackgroundToList(bool isPostcard, IMainWindow mainWindow)
    {
        var pickFile = await _filePickerService.PickFilePath(Models.FileTypes.NonTransparentImages);
        var pathToCopy = isPostcard ? _applicationInfoService.PostcardBackgroundPath : _applicationInfoService.StripeBackgroundPath;
        var imageSize = _imageManager.GetImageSizeFromFile(pickFile);
        if (imageSize.HasValue)
        {
            var ratio = (double)imageSize.Value.Width / imageSize.Value.Height;
            if (((isPostcard && ratio != Consts.Background.PostcardBackgroundRatio)
                || (!isPostcard && ratio != Consts.Background.StripeBackgroundRatio))
                && !await _messageBoxService.ShowYesNo(Assets.UI.wrongImageRatioTitle, Assets.UI.wrongImageRatioMessage, mainWindow))
            {
                return;
            }
        }
        else
        {
            return;
        }
        _filesManager.CopyFile(pickFile, pathToCopy);
        if (isPostcard)
        {
            PopulateBackgroundList(pathToCopy, PostcardBackgroundInfo);
        }
        else
        {
            PopulateBackgroundList(pathToCopy, StripeBackgroundInfo);
        }
    }

    [RelayCommand]
    private Task AddPostcardBackgroundToList(IMainWindow mainWindow)
    {
        return AddBackgroundToList(true, mainWindow);
    }

    [RelayCommand]
    private Task AddStripeBackgroundToList(IMainWindow mainWindow)
    {
        return AddBackgroundToList(false, mainWindow);
    }

    [RelayCommand]
    private void RemoveBackgroundFromList(bool postcardList)
    {
        if (postcardList)
        {
            DeleteItem(PostcardSelectedItem, PostcardBackgroundInfo);
        }
        else
        {
            DeleteItem(StripeSelectedItem, StripeBackgroundInfo);
        }
    }

    private void DeleteItem(string? selectedItem, BackgroundInfo backgroundInfo)
    {
        if (selectedItem != null)
        {
            _filesManager.DeleteFile(selectedItem);
            backgroundInfo.BackgroundPathsList.Remove(selectedItem);

            if (backgroundInfo.BackgroundPath == selectedItem)
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
}
