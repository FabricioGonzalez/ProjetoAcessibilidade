using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using Common;

using ProjectAvalonia.ViewModels.Navigation;

using ReactiveUI;

namespace ProjectAvalonia.ViewModels.Dialogs;

[NavigationMetaData(Title = Constants.ShuttingDownLabel, LocalizedTitle = "ShutdownDialogTitleLabel")]
public partial class ShuttingDownViewModel : RoutableViewModel
{
    private readonly ApplicationViewModel _applicationViewModel;
    private readonly bool _restart;

    public ShuttingDownViewModel(
        ApplicationViewModel applicationViewModel
        , bool restart
    )
    {
        _applicationViewModel = applicationViewModel;
        _restart = restart;
        NextCommand = CancelCommand;
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;
    public override MenuViewModel? ToolBar => null;
    protected override void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
    ) =>
        Observable
            .Interval(TimeSpan.FromSeconds(5))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ =>
            {
                if (_applicationViewModel.CanShutdown())
                {
                    Navigate().Clear();
                    _applicationViewModel.Shutdown(_restart);
                }
            })
            .DisposeWith(disposables);
}