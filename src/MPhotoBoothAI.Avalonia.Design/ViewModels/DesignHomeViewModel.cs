using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Avalonia.Design.ViewModels;

public class DesignHomeViewModel : HomeViewModel
{
    public DesignHomeViewModel() : base(new LanguageViewModel(new Mock<IUserSettingsService>().Object, new Mock<IAppRestarterService>().Object))
    {
    }
}
