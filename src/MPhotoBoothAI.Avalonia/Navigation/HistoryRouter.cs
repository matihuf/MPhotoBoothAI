using MPhotoBoothAI.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPhotoBoothAI.Avalonia.Navigation;

public class HistoryRouter<TViewModelBase>(Func<Type, TViewModelBase> createViewModel) : Router<TViewModelBase>(createViewModel), INavigationService<TViewModelBase> where TViewModelBase : class
{
    private int _historyIndex = -1;
    private List<TViewModelBase> _history = [];
    private readonly uint _historyMaxSize = 100;

    public bool HasNext => _history.Count > 0 && _historyIndex < _history.Count - 1;
    public bool HasPrev => _historyIndex > 0;

    private void Push(TViewModelBase item)
    {
        if (HasNext)
        {
            _history = _history.Take(_historyIndex + 1).ToList();
        }
        _history.Add(item);
        _historyIndex = _history.Count - 1;
        if (_history.Count > _historyMaxSize)
        {
            _history.RemoveAt(0);
        }
    }

    private void Go(int offset = 0)
    {
        if (offset == 0)
        {
            return;
        }

        var newIndex = _historyIndex + offset;
        if (newIndex < 0 || newIndex > _history.Count - 1)
        {
            return;
        }
        ClearCurrent();
        _historyIndex = newIndex;
        var viewModel = _history[_historyIndex];
        CurrentViewModel = viewModel;
    }

    public void Back()
    {
        if (HasPrev)
        {
            Go(-1);
        }
    }

    public void Forward()
    {
        if (HasPrev)
        {
            Go(1);
        }
    }

    public override T GoTo<T>()
    {
        ClearCurrent();
        var destination = InstantiateViewModel<T>();
        CurrentViewModel = destination;
        Push(destination);
        return destination;
    }

    public void GoTo(Type type)
    {
        ClearCurrent();
        var destination = InstantiateViewModel(type);
        CurrentViewModel = destination;
        Push(destination);
    }

    private void ClearCurrent() => (_currentViewModel as IDisposable)?.Dispose();
}
