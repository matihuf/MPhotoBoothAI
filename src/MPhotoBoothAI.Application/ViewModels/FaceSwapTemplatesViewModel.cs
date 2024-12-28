using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.Entities;
using MPhotoBoothAI.Models.FaceSwaps;
using MPhotoBoothAI.Models.WindowParameters;
using MPhotoBoothAI.Models.WindowResults;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Application.ViewModels;
public partial class FaceSwapTemplatesViewModel : ViewModelBase
{
    public ObservableCollection<FaceSwapTemplateGroupEntity> Groups { get; set; }
    public ObservableCollection<FaceSwapTemplate> Templates { get; set; } = [];

    [ObservableProperty]
    private FaceSwapTemplateGroupEntity? _selectedGroup;

    [ObservableProperty]
    private bool _isGroupInEdit;

    private readonly IDatabaseContext _databaseContext;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IWindowService _windowsService;
    private readonly IFaceSwapTemplateFileService _faceSwapTemplateFileService;
    private FaceSwapTemplateGroupEntity? _beforeEditGroup;

    public FaceSwapTemplatesViewModel(IDatabaseContext databaseContext, IMessageBoxService messageBoxService, IWindowService windowService,
        IFaceSwapTemplateFileService faceSwapTemplateFileService)
    {
        _databaseContext = databaseContext;
        _messageBoxService = messageBoxService;
        _windowsService = windowService;
        _faceSwapTemplateFileService = faceSwapTemplateFileService;
        _databaseContext.FaceSwapTemplateGroups.OrderBy(x => x.CreatedAt).Load();
        Groups = _databaseContext.FaceSwapTemplateGroups.Local.ToObservableCollection();
        SelectedGroup = Groups.FirstOrDefault();
        _beforeEditGroup = (FaceSwapTemplateGroupEntity?)SelectedGroup?.Clone();
        IsGroupInEdit = false;
    }

    [RelayCommand]
    private async Task AddGroup(IMainWindow mainWindow)
    {
        string groupName = await _messageBoxService.ShowInput(Assets.UI.addGroup, Assets.UI.name, mainWindow);
        if (string.IsNullOrEmpty(groupName))
        {
            return;
        }
        var faceSwapTemplateGroup = new FaceSwapTemplateGroupEntity { Name = groupName };
        Groups.Add(faceSwapTemplateGroup);
        SelectedGroup = faceSwapTemplateGroup;
        await SaveChanges();
    }

    [RelayCommand]
    private async Task DeleteGroup(IMainWindow mainWindow)
    {
        if (SelectedGroup != null && await _messageBoxService.ShowYesNo(Assets.UI.deleteGroup, Assets.UI.deleteGroupDesc, mainWindow))
        {
            Groups.Remove(SelectedGroup);
            await SaveChanges();
            SelectedGroup = Groups.FirstOrDefault();
        }
    }

    [RelayCommand]
    private void EditGroup()
    {
        _beforeEditGroup = (FaceSwapTemplateGroupEntity?)SelectedGroup?.Clone();
        IsGroupInEdit = !IsGroupInEdit;
    }

    [RelayCommand]
    private void CancelEditGroup()
    {
        if (SelectedGroup == null || _beforeEditGroup == null)
        {
            return;
        }
        var editedIndex = Groups.IndexOf(SelectedGroup);
        Groups[editedIndex] = _beforeEditGroup;
        SelectedGroup = Groups[editedIndex];
        IsGroupInEdit = !IsGroupInEdit;
    }

    [RelayCommand]
    private async Task SaveEditGroup()
    {
        await SaveChanges();
        IsGroupInEdit = !IsGroupInEdit;
    }

    [RelayCommand]
    private async Task AddTemplate(IMainWindow mainWindow)
    {
        if (SelectedGroup == null)
        {
            return;
        }
        var result = await _windowsService.Open<AddFaceSwapTemplateResults, AddFaceSwapTemplateParameters>(typeof(AddFaceSwapTemplateViewModel),
             mainWindow, new AddFaceSwapTemplateParameters(SelectedGroup.Id));
        if (result != null)
        {
            AddTemplate(SelectedGroup.Id, result.TemplateId, result.Faces);
        }
    }

    private void AddTemplate(int groupId, int templateId, int faces)
    {
        string templatePath = _faceSwapTemplateFileService.GetFullTemplateThumbnailPath(groupId, templateId);
        if (File.Exists(templatePath))
        {
            Templates.Add(new FaceSwapTemplate(templatePath, faces));
        }
    }

    partial void OnSelectedGroupChanged(FaceSwapTemplateGroupEntity? value)
    {
        if (value == null)
        {
            return;
        }
        Templates.Clear();
        foreach (var item in _databaseContext.FaceSwapTemplates.AsNoTracking().Where(x => x.FaceSwapTemplateGroupId == value.Id).OrderBy(x => x.CreatedAt))
        {
            AddTemplate(item.FaceSwapTemplateGroupId, item.Id, item.Faces);
        }
    }

    [RelayCommand]
    private Task<int> SaveChanges() => _databaseContext.SaveChangesAsync();
}
