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
    private FaceSwapTemplateGroupEntity? _beforeEditGroup;

    public FaceSwapTemplatesViewModel(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        _databaseContext.FaceSwapTemplateGroups.OrderBy(x => x.CreatedAt).Load();
        Groups = _databaseContext.FaceSwapTemplateGroups.Local.ToObservableCollection();
        SelectedGroup = Groups.FirstOrDefault();
        _beforeEditGroup = (FaceSwapTemplateGroupEntity?)SelectedGroup?.Clone();
        IsGroupInEdit = false;
    }

    [RelayCommand]
    private async Task AddGroup()
    {
        var faceSwapTemplateGroup = new FaceSwapTemplateGroupEntity { Name = Assets.UI.newGroup };
        Groups.Add(faceSwapTemplateGroup);
        SelectedGroup = faceSwapTemplateGroup;
        await SaveChanges();
    }

    [RelayCommand]
    private async Task DeleteGroup()
    {
        if (SelectedGroup != null)
        {
            Groups.Remove(SelectedGroup);
            await SaveChanges();
            SelectedGroup = Groups.FirstOrDefault();
        }
    }

    [RelayCommand]
    private void AddTemplate()
    {
        Templates.Add(new FaceSwapTemplateEntity());
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
    private Task SaveChanges() => _databaseContext.SaveChangesAsync();
}
