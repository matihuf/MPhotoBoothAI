using MPhotoBoothAI.Models.UI;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Avalonia.Design;
public static class DesignTimeData
{
    public static ObservableCollection<CrudListItem> CrudListItems => new([
        new CrudListItem("Group 1"),
        new CrudListItem("Group 2"),
        new CrudListItem("Group 3"),
        new CrudListItem("Group 1"),
        new CrudListItem("Group 2"),
        new CrudListItem("Group 3"),
        new CrudListItem("Group 1"),
        new CrudListItem("Group 2"),
        new CrudListItem("Group 3"),
        new CrudListItem("Group 1"),
        new CrudListItem("Group 2"),
        new CrudListItem("Group 3")
        ]);
}