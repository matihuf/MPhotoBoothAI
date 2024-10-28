using CommunityToolkit.Mvvm.ComponentModel;

namespace MPhotoBoothAI.Models.Camera
{
    public partial class CameraSetting : ObservableObject
    {
        [ObservableProperty]
        private string? _current;

        [ObservableProperty]
        private IEnumerable<string>? _availableValues;
    }
}
