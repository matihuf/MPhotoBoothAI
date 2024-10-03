namespace MPhotoBoothAI.Application.Interfaces;

public interface IHistoryRouter<TViewModelBase>
{
    event Action<TViewModelBase>? CurrentViewModelChanged;
    T GoTo<T>() where T : TViewModelBase;
    void GoTo(Type type);
    TViewModelBase? Back();
    TViewModelBase? Forward();
}
