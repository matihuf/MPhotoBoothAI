using System;

namespace MPhotoBoothAI.Avalonia.Navigation;

public class Router<TViewModelBase>(Func<Type, TViewModelBase> createViewModel) where TViewModelBase : class
{
    protected TViewModelBase _currentViewModel = default!;
    protected readonly Func<Type, TViewModelBase> CreateViewModel = createViewModel;
    public event Action<TViewModelBase>? CurrentViewModelChanged;

    protected TViewModelBase CurrentViewModel
    {
        set
        {
            if (value == _currentViewModel) return;
            _currentViewModel = value;
            OnCurrentViewModelChanged(value);
        }
    }

    private void OnCurrentViewModelChanged(TViewModelBase viewModel)
    {
        CurrentViewModelChanged?.Invoke(viewModel);
    }

    public virtual T GoTo<T>() where T : TViewModelBase
    {
        var viewModel = InstantiateViewModel<T>();
        CurrentViewModel = viewModel;
        return viewModel;
    }

    protected T InstantiateViewModel<T>() where T : TViewModelBase => (T)CreateViewModel(typeof(T));

    protected TViewModelBase InstantiateViewModel(Type type) => CreateViewModel(type);
}