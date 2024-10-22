namespace MPhotoBoothAI.Models.UI;
public class CrudListItem(string label, bool isNew = false)
{
    public string Label { get; set; } = label;
    public bool IsNew { get; set; } = isNew;
}
