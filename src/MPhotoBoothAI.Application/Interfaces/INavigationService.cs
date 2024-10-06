namespace MPhotoBoothAI.Application.Interfaces;

public interface INavigationService<TViewModelBase>
{
    event Action<TViewModelBase>? CurrentViewModelChanged;
    T GoTo<T>() where T : TViewModelBase;
    void GoTo(Type type);
    void Forward();
    void Back();
}
