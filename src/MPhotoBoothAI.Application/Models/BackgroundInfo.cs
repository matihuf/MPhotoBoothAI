using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Application.Models;
public partial class BackgroundInfo : ObservableObject
{
    [ObservableProperty]
    private string? _backgroundPath;

    [ObservableProperty]
    private string? _selectedItem;

    [ObservableProperty]
    private ObservableCollection<string> _backgroundPathsList = [];
}
