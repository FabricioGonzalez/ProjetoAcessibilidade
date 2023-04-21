using System.Reactive.Disposables;
using ProjectAvalonia.Common.ViewModels;

namespace ProjectAvalonia.ViewModels;

public class ActivatableViewModel : ViewModelBase
{
    protected virtual void OnActivated(
        CompositeDisposable disposables
    )
    {
    }

    public void Activate(
        CompositeDisposable disposables
    ) => OnActivated(disposables: disposables);
}