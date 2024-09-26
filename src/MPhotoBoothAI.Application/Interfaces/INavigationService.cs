using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Application.Interfaces;

public interface INavigationService
{
    T GoTo<T>() where T : ViewModelBase;
    void GoTo(Type type);
    void Forward();
    void Back();
}
