using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Application.Interfaces;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Application.ViewModels;
public partial class DesignPrintTemplateViewModel : ViewModelBase
{
    private readonly IFilePickerService _filePickerService;

    private readonly IFilesManager _filesManager;

    private readonly IApplicationInfoService _applicationInfoService;

    private readonly IImageManager _imageManager;

    private readonly IMessageBoxService _messageBoxService;

    [ObservableProperty]
    private ObservableCollection<string> _stripeBackgroundsPaths = [];

    [ObservableProperty]
    private ObservableCollection<string> _postcardBackgroundsPaths = [];

    [ObservableProperty]
    private string _postcardSelectedItem = string.Empty;

    [ObservableProperty]
    private string _stripeSelectedItem = string.Empty;

    [ObservableProperty]
    private string _stripeBackgroundPath;

    [ObservableProperty]
    private string _postcardBackgroundPath;

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
        PopulateBackgroundList(_applicationInfoService.PostcardBackgroundPath, PostcardBackgroundsPaths);
        PopulateBackgroundList(_applicationInfoService.StripeBackgroundPath, StripeBackgroundsPaths);
        StripeBackgroundPath = StripeBackgroundsPaths.Count > 0 ? StripeBackgroundsPaths[0] : string.Empty;
        PostcardBackgroundPath = PostcardBackgroundsPaths.Count > 0 ? PostcardBackgroundsPaths[0] : string.Empty;
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
            PopulateBackgroundList(pathToCopy, PostcardBackgroundsPaths);
        }
        else
        {
            PopulateBackgroundList(pathToCopy, StripeBackgroundsPaths);
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

    private void PopulateBackgroundList(string pathToCopy, ObservableCollection<string> collection)
    {
        collection.Clear();
        foreach (var path in _filesManager.GetFilesNames(pathToCopy))
        {
            collection.Add(path);
        }
    }
}
