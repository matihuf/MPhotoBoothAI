using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Models.UI;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Application.ViewModels;
public partial class FaceSwapTemplatesViewModel : ViewModelBase
{
    public ObservableCollection<CrudListItem> Groups { get; set; } = [];
    public ObservableCollection<CrudListItem> Templates { get; set; } = [];

    [ObservableProperty]
    private CrudListItem? _selectedGroup;

    [ObservableProperty]
    private string? _title;

    public FaceSwapTemplatesViewModel()
    {
        SelectedGroup = Groups.FirstOrDefault();
    }

    [RelayCommand]
    private void AddGroup()
    {
        var newItemsCount = Groups.Count(x => x.IsNew);
        string label = newItemsCount == 0 ? Assets.UI.newGroup : $"{Assets.UI.newGroup} ({newItemsCount})";
        var newEntry = new CrudListItem(label, true);
        Groups.Add(newEntry);
        SelectedGroup = newEntry;
    }

    [RelayCommand]
    private void DeleteGroup(CrudListItem crudListItem)
    {
        Groups.Remove(crudListItem);
        SelectedGroup = Groups.FirstOrDefault();
    }

    partial void OnSelectedGroupChanged(CrudListItem? value)
    {
        Title = value?.Label ?? string.Empty;
    }

    [RelayCommand]
    private void AddTemplate()
    {
        Templates.Add(new CrudListItem(string.Empty, true));
    }
}
