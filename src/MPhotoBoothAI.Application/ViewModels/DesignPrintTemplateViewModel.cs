using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Application.ViewModels;
public partial class DesignPrintTemplateViewModel : ViewModelBase
{
    private readonly IFilesManager _filesManager;

    private readonly IDatabaseContext _dbContext;

    private readonly IApplicationInfoService _applicationInfoService;

    private readonly IImageManager _imageManager;

    private readonly IMessageBoxService _messageBoxService;

    private readonly Dictionary<FormatTypes, string> _backgroundDir = [];

    [ObservableProperty]
    private bool _notSavedChange;

    [ObservableProperty]
    private IFilePickerService _filePickerService;

    [ObservableProperty]
    private List<LayoutFormatEntity> _layoutFormat = [];

    [ObservableProperty]
    private int _id = 0;

    [ObservableProperty]
    private BackgroundInfo _backgroundInfo = new();

    [ObservableProperty]
    private LayoutFormatEntity _selectedLayoutFormat = new();

    [ObservableProperty]
    private LayoutDataEntity _selectedLayoutData = new();

    private Dictionary<FormatTypes, BackgroundInfo> _formats = [];

    public DesignPrintTemplateViewModel(IFilePickerService filePickerService,
        IFilesManager filesManager,
        IDatabaseContext dbContext,
        IApplicationInfoService applicationInfoService,
        IImageManager imageManager,
        IMessageBoxService messageBoxService)
    {
        FilePickerService = filePickerService;
        _filesManager = filesManager;
        _dbContext = dbContext;
        _applicationInfoService = applicationInfoService;
        _imageManager = imageManager;
        _messageBoxService = messageBoxService;
        BuildProperties();
    }

    private void BuildProperties()
    {
        LayoutFormat = _dbContext.LayoutFormat.ToList();
        foreach (FormatTypes format in Enum.GetValues(typeof(FormatTypes)))
        {
            _formats.Add(format, new BackgroundInfo());
            _backgroundDir.Add(format, Path.Combine(_applicationInfoService.BackgroundDirectory, format.ToString()));
        }
        Id = 1;
    }

    partial void OnIdChanged(int value)
    {
        FormatTypes type = (FormatTypes)value;
        BackgroundInfo = _formats[type];
        SelectedLayoutFormat = LayoutFormat.First(x => x.Id == Id);
        SelectedLayoutData = _dbContext.LayoutDatas
            .Include(x => x.PhotoLayoutData)
            .Include(x => x.OverlayImageData)
            .First(x => x.Id == Id);
        PopulateBackgroundList(_backgroundDir[type]);
    }

    private void PopulateBackgroundList(string pathToCopy)
    {
        BackgroundInfo.BackgroundPathsList.Clear();
        foreach (var path in _filesManager.GetFiles(pathToCopy))
        {
            BackgroundInfo.BackgroundPathsList.Add(path);
        }
        if (String.IsNullOrEmpty(BackgroundInfo.BackgroundPath) && BackgroundInfo.BackgroundPathsList.Count > 0)
        {
            BackgroundInfo.BackgroundPath = BackgroundInfo.BackgroundPathsList[0];
        }
    }

    [RelayCommand]
    private async Task AddBackgroundToList()
    {
        var pickFile = await FilePickerService.PickFilePath(Models.FileTypes.NonTransparentImages);
        var pathToCopy = _backgroundDir[(FormatTypes)Id];
        var imageSize = _imageManager.GetImageSizeFromFile(pickFile);
        if (imageSize.HasValue)
        {
            var formatRatio = _dbContext.LayoutFormat.First(x => x.Id == Id).FormatRatio;
            var ratio = (double)imageSize.Value.Width / imageSize.Value.Height;
            if (ratio < formatRatio * 1.01
                && ratio > formatRatio * 0.99
                && !await _messageBoxService.ShowYesNo(Assets.UI.wrongImageRatioTitle, Assets.UI.wrongImageRatioMessage, null))
            {
                return;
            }
        }
        else
        {
            return;
        }
        _filesManager.CopyFile(pickFile, pathToCopy);
        PopulateBackgroundList(pathToCopy);
    }

    [RelayCommand]
    private async Task ChangeFormatIndex(int index)
    {
        if (!NotSavedChange)
        {
            Id = index;
            return;
        }
        if (await _messageBoxService.ShowYesNo(Assets.UI.notSavedChangesTittle, Assets.UI.notSavedChangesMessage, null))
        {
            Id = index;
            NotSavedChange = false;
        }
    }

    [RelayCommand]
    private void RemoveBackgroundFromList()
    {
        if (BackgroundInfo.SelectedItem != null)
        {
            var selectedValueCopy = BackgroundInfo.SelectedItem;
            _filesManager.DeleteFile(BackgroundInfo.SelectedItem);
            BackgroundInfo.BackgroundPathsList.Remove(BackgroundInfo.SelectedItem);
            if (BackgroundInfo.BackgroundPath == selectedValueCopy)
            {
                if (BackgroundInfo.BackgroundPathsList.Count > 0)
                {
                    BackgroundInfo.BackgroundPath = BackgroundInfo.BackgroundPathsList[0];
                    return;
                }
                BackgroundInfo.BackgroundPath = null;
            }
        }
    }

    [RelayCommand]
    private void LoadNextBackground()
    {
        var count = BackgroundInfo.BackgroundPathsList.Count;
        if (count > 0 && !String.IsNullOrEmpty(BackgroundInfo.BackgroundPath))
        {
            var index = BackgroundInfo.BackgroundPathsList.IndexOf(BackgroundInfo.BackgroundPath) + 1;
            if (index == count)
            {
                index = 0;
            }
            BackgroundInfo.BackgroundPath = BackgroundInfo.BackgroundPathsList[index];
        }
    }

    [RelayCommand]
    private async Task SaveLayout()
    {
        _dbContext.SaveChanges();
        NotSavedChange = false;
        await _messageBoxService.ShowInfo(Assets.UI.savedChanges, Assets.UI.savedChanges, null);
    }
}
