using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPhotoBoothAI.Models.UI;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Application.ViewModels;
public partial class FaceSwapTemplatesViewModel : ViewModelBase
{
    public ObservableCollection<CrudListItem> Groups { get; set; } = [];

    [ObservableProperty]
    private CrudListItem? _selectedGroup;

    [RelayCommand]
    private void AddGroup()
    {
        var newItemsCount = Groups.Count(x => x.IsNew);
        string label = newItemsCount == 0 ? Assets.UI.newGroup : $"{Assets.UI.newGroup} ({newItemsCount})";
        Groups.Add(new CrudListItem(label, true));
    }

    [RelayCommand]
    private void DeleteGroup(CrudListItem crudListItem)
    {
        Groups.Remove(crudListItem);
    }
}
