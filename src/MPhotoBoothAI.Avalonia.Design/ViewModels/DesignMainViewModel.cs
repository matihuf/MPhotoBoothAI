using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Avalonia.Design.ViewModels;

public class DesignMainViewModel : MainViewModel
{
    public DesignMainViewModel() : base(new Mock<INavigationService<ViewModelBase>>().Object)
    {
    }
}
