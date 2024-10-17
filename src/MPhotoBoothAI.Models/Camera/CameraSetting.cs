using CommunityToolkit.Mvvm.ComponentModel;

namespace MPhotoBoothAI.Models.Camera
{
    public partial class CameraSetting : ObservableObject
    {
        [ObservableProperty]
        private string? _current;

        public IEnumerable<string>? AvailableValues { get; set; }
    }
}
