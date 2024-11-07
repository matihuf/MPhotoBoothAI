using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.Entities;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Application.ViewModels;
public partial class FaceSwapTemplatesViewModel : ViewModelBase
{
    public ObservableCollection<FaceSwapTemplateGroupEntity> Groups { get; set; }
    public ObservableCollection<FaceSwapTemplateEntity> Templates { get; set; } = [];

    [ObservableProperty]
    private FaceSwapTemplateGroupEntity? _selectedGroup;

    [ObservableProperty]
    private bool _isGroupInEdit;

    private readonly IDatabaseContext _databaseContext;
    private readonly IMessageBoxService _messageBoxService;
    private FaceSwapTemplateGroupEntity? _beforeEditGroup;

    public FaceSwapTemplatesViewModel(IDatabaseContext databaseContext, IMessageBoxService messageBoxService)
    {
        _databaseContext = databaseContext;
        _messageBoxService = messageBoxService;
        _databaseContext.FaceSwapTemplateGroups.OrderBy(x => x.CreatedAt).Load();
        Groups = _databaseContext.FaceSwapTemplateGroups.Local.ToObservableCollection();
        SelectedGroup = Groups.FirstOrDefault();
        _beforeEditGroup = (FaceSwapTemplateGroupEntity?)SelectedGroup?.Clone();
        IsGroupInEdit = false;
    }

    [RelayCommand]
    private async Task AddGroup()
    {
        string groupName = await _messageBoxService.ShowInput(Assets.UI.addGroup, Assets.UI.name);
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
    private async Task DeleteGroup()
    {
        if (SelectedGroup != null && await _messageBoxService.ShowYesNo(Assets.UI.deleteGroup, Assets.UI.deleteGroupDesc))
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
    private async Task AddTemplate()
    {
        if (SelectedGroup == null)
        {
            return;
        }
        var newTemplate = new FaceSwapTemplateEntity
        {
            FaceSwapTemplateGroupId = SelectedGroup.Id,
            FileName = string.Empty
        };
        _databaseContext.FaceSwapTemplates.Add(newTemplate);
        await SaveChanges();
        Templates.Add(newTemplate);
    }

    partial void OnSelectedGroupChanged(FaceSwapTemplateGroupEntity? value)
    {
        if (value == null)
        {
            return;
        }
        Templates.Clear();
        foreach (var item in _databaseContext.FaceSwapTemplates.Where(x => x.FaceSwapTemplateGroupId == value.Id).OrderBy(x => x.CreatedAt).ToList())
        {
            Templates.Add(item);
        }
    }

    [RelayCommand]
    private Task<int> SaveChanges() => _databaseContext.SaveChangesAsync();
}
