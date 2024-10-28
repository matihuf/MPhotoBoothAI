using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Models.Entities;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Application.ViewModels;
public partial class FaceSwapTemplatesViewModel : ViewModelBase
{
    public ObservableCollection<FaceSwapTemplateGroupEntity> Groups { get; set; } = [];
    public ObservableCollection<FaceSwapTemplateEntity> Templates { get; set; } = [];

    [ObservableProperty]
    private FaceSwapTemplateGroupEntity? _selectedGroup;

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private bool _isGroupInEdit;

    public FaceSwapTemplatesViewModel()
    {
        SelectedGroup = Groups.FirstOrDefault();
        IsGroupInEdit = false;
    }

    [RelayCommand]
    private void AddGroup()
    {
        // Groups.Add(newEntry);
        //  SelectedGroup = newEntry;
    }

    [RelayCommand]
    private void DeleteGroup(FaceSwapTemplateGroupEntity faceSwapTemplateGroupEntity)
    {
        Groups.Remove(faceSwapTemplateGroupEntity);
        SelectedGroup = Groups.FirstOrDefault();
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
