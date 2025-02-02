using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Models.Entities;
using MPhotoBoothAI.Models.Enums;

namespace MPhotoBoothAI.Application.ViewModels;
public partial class DesignPrintTemplateViewModel : ViewModelBase
{
    private const string BackgroundFileName = "background.jpg";

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
    private string? _backgroundPath = null;

    [ObservableProperty]
    private LayoutFormatEntity _selectedLayoutFormat = new();

    [ObservableProperty]
    private LayoutDataEntity _selectedLayoutData = new();

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
        LayoutFormat = _dbContext.LayoutFormat.AsNoTracking().ToList();
        foreach (FormatTypes format in Enum.GetValues(typeof(FormatTypes)))
        {
            _backgroundDir.Add(format, Path.Combine(_applicationInfoService.BackgroundDirectory, format.ToString()));
        }
        Id = 1;
    }

    partial void OnIdChanged(int value)
    {
        FormatTypes type = (FormatTypes)value;
        SelectedLayoutFormat = LayoutFormat.First(x => x.Id == Id);
        SelectedLayoutData = _dbContext.LayoutDatas
            .Include(x => x.PhotoLayoutData)
            .Include(x => x.OverlayImageData)
            .First(x => x.Id == Id);
        BackgroundPath = Path.Combine(_backgroundDir[type], BackgroundFileName);
    }

    [RelayCommand]
    private async Task SetBackground()
    {
        var pickFile = await FilePickerService.PickFilePath([FilePickerFileType.NonTransparentImage]);
        if (String.IsNullOrEmpty(pickFile))
        {
            return;
        }
        var pathToCopy = _backgroundDir[(FormatTypes)Id];
        var imageSize = _imageManager.GetImageSizeFromFile(pickFile);
        if (!imageSize.HasValue)
        {
            return;
        }
        var formatRatio = SelectedLayoutFormat.FormatRatio;
        var ratio = imageSize.Value.Height / (double)imageSize.Value.Width;
        if ((ratio > formatRatio * 1.01 || ratio < formatRatio * 0.99) && !await _messageBoxService.ShowYesNo(Assets.UI.wrongImageRatioTitle, Assets.UI.wrongImageRatioMessage, null))
        {
            return;
        }

        _filesManager.CopyFile(pickFile, pathToCopy, BackgroundFileName);
        BackgroundPath = null;
        BackgroundPath = Path.Combine(pathToCopy, BackgroundFileName);
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
    private async Task SaveLayout()
    {
        _dbContext.SaveChanges();
        NotSavedChange = false;
        await _messageBoxService.ShowInfo(Assets.UI.savedChanges, Assets.UI.savedChanges, null);
    }
}
