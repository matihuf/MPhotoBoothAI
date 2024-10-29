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
    private string? _title;

    [ObservableProperty]
    private bool _isGroupInEdit;

    private readonly IDatabaseContext _databaseContext;

    public FaceSwapTemplatesViewModel(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        _databaseContext.FaceSwapTemplateGroups.Load();
        Groups = _databaseContext.FaceSwapTemplateGroups.Local.ToObservableCollection();
        SelectedGroup = Groups.FirstOrDefault();
        IsGroupInEdit = false;
    }

    [RelayCommand]
    private async Task AddGroup()
    {
        var faceSwapTemplateGroup = new FaceSwapTemplateGroupEntity { Name = Assets.UI.newGroup };
        Groups.Add(faceSwapTemplateGroup);
        SelectedGroup = faceSwapTemplateGroup;
        await _databaseContext.SaveChangesAsync();
    }

    [RelayCommand]
    private async Task DeleteGroup()
    {
        if (SelectedGroup != null)
        {
            Groups.Remove(SelectedGroup);
            await _databaseContext.SaveChangesAsync();
            SelectedGroup = Groups.FirstOrDefault();
        }
    }

    partial void OnSelectedGroupChanged(FaceSwapTemplateGroupEntity? value)
    {
        Title = value?.Name ?? string.Empty;
    }

    [RelayCommand]
    private void AddTemplate()
    {
        Templates.Add(new FaceSwapTemplateEntity());
    }

    [RelayCommand]
    private void EditGroup() => IsGroupInEdit = !IsGroupInEdit;
}
