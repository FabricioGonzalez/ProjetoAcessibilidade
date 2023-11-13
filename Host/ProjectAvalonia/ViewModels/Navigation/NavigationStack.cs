using System.Collections.Generic;
using System.Linq;
using ProjectAvalonia.Common.ViewModels;

namespace ProjectAvalonia.ViewModels.Navigation;

public partial class NavigationStack<T>
    : ViewModelBase
        , INavigationStack<T>
    where T : class, INavigatable
{
    private readonly Stack<T> _backStack;
    [AutoNotify] private bool _canNavigateBack;
    [AutoNotify] private T? _currentPage;

    private bool _operationsEnabled = true;

    protected NavigationStack()
    {
        _backStack = new Stack<T>();
    }

    protected IEnumerable<T> Stack => _backStack;

    public virtual void Clear() => Clear(keepRoot: false);

    public void BackTo(
        T viewmodel
    )
    {
        if (CurrentPage == viewmodel)
        {
            return;
        }

        if (_backStack.Contains(item: viewmodel))
        {
            var oldPage = CurrentPage;

            while (_backStack.Pop() != viewmodel)
            {
            }

            NavigationOperation(oldPage: oldPage, oldInStack: false, newPage: viewmodel, newInStack: true);
        }
    }

    public void BackTo<TViewModel>()
        where TViewModel : T
    {
        var previous = _backStack.Reverse().SingleOrDefault(predicate: x => x is TViewModel);

        if (previous is not null)
        {
            BackTo(viewmodel: previous);
        }
    }

    public void Back()
    {
        if (_backStack.Count > 0)
        {
            var oldPage = CurrentPage;

            CurrentPage = _backStack.Pop();

            NavigationOperation(oldPage: oldPage, oldInStack: false, newPage: CurrentPage, newInStack: true);
        }
        else
        {
            Clear(); // in this case only CurrentPage might be set and Clear will provide correct behavior.
        }
    }

    public void To(
        T viewmodel
        , NavigationMode mode = NavigationMode.Normal
        , object? Parameter = null
    )
    {
        var oldPage = CurrentPage;

        var oldInStack = true;
        var newInStack = false;

        switch (mode)
        {
            case NavigationMode.Normal:
                if (oldPage is not null)
                {
                    _backStack.Push(item: oldPage);
                }

                break;

            case NavigationMode.Clear:
                oldInStack = false;
                _operationsEnabled = false;
                Clear();
                _operationsEnabled = true;
                break;

            case NavigationMode.Skip:
                // Do not push old page on the back stack.
                break;
        }

        NavigationOperation(oldPage: oldPage, oldInStack: oldInStack, newPage: viewmodel, newInStack: newInStack
            , Parameter: Parameter);
    }

    protected virtual void OnNavigated(
        T? oldPage
        , bool oldInStack
        , T? newPage
        , bool newInStack
    )
    {
    }

    protected virtual void OnPopped(
        T page
    )
    {
    }

    private void NavigationOperation(
        T? oldPage
        , bool oldInStack
        , T? newPage
        , bool newInStack
        , object Parameter = null
    )
    {
        if (_operationsEnabled)
        {
            oldPage?.OnNavigatedFrom(isInHistory: oldInStack);
        }

        CurrentPage = newPage;

        if (!oldInStack && oldPage is not null)
        {
            OnPopped(page: oldPage);
        }

        if (_operationsEnabled)
        {
            OnNavigated(oldPage: oldPage, oldInStack: oldInStack, newPage: newPage, newInStack: newInStack);
        }

        if (_operationsEnabled && newPage is not null)
        {
            if (Parameter is null)
            {
                newPage.OnNavigatedTo(isInHistory: newInStack);
            }

            else
            {
                newPage.OnNavigatedTo(isInHistory: newInStack, Parameter: Parameter);
            }
        }

        UpdateCanNavigateBack();
    }

    protected virtual void Clear(
        bool keepRoot
    )
    {
        var root = _backStack.Count > 0 ? _backStack.Last() : CurrentPage;

        if ((keepRoot && CurrentPage == root) || (!keepRoot && _backStack.Count == 0 && CurrentPage is null))
        {
            return;
        }

        var oldPage = CurrentPage;

        var oldItems = _backStack.ToList();

        _backStack.Clear();

        if (keepRoot)
        {
            foreach (var item in oldItems)
            {
                if (item != root)
                {
                    OnPopped(page: item);
                }
            }

            CurrentPage = root;
        }
        else
        {
            foreach (var item in oldItems)
            {
                if (item is INavigatable navigatable)
                {
                    navigatable.OnNavigatedFrom(isInHistory: false);
                }

                OnPopped(page: item);
            }

            CurrentPage = null;
        }

        NavigationOperation(oldPage: oldPage, oldInStack: false, newPage: CurrentPage
            , newInStack: CurrentPage is not null);
    }

    private void UpdateCanNavigateBack() => CanNavigateBack = _backStack.Count > 0;
}