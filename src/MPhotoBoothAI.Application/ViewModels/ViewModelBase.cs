using CommunityToolkit.Mvvm.ComponentModel;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
#if DEBUG
    private bool _isDebug = true;
#else
    private bool _isDebug = false;
#endif
}
