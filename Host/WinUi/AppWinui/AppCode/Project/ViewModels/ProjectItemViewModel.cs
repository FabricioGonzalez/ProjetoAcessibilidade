using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;

using AppWinui.AppCode.Project.Contracts;
using AppWinui.AppCode.Project.States;

using ReactiveUI;

namespace AppWinui.AppCode.Project.ViewModels;
public class ProjectItemViewModel : ReactiveObject, IActivatableViewModel, IProjectItemCommands
{
    public ICommand OpenProjectItemCommand => ReactiveCommand.Create(() =>
    {

    });
    public ICommand SaveProjectItemCommand => ReactiveCommand.Create(() =>
    {

    });
    private ProjectItemState _project;
    public ProjectItemState Project
    {
        get => _project;
        set => this.RaiseAndSetIfChanged(ref _project, value, nameof(Project));
    }


    public ViewModelActivator Activator
    {
        get;
    }

    public ProjectItemViewModel()
    {
        #region Activator
        Activator = new ViewModelActivator();
        this.WhenActivated(disposables =>
        {
            // Use WhenActivated to execute logic
            // when the view model gets activated.
            HandleActivation();

            // Or use WhenActivated to execute logic
            // when the view model gets deactivated.
            Disposable
                .Create(() => HandleDeactivation())
                .DisposeWith(disposables);

            // Here we create a hot observable and 
            // subscribe to its notifications. The
            // subscription should be disposed when we'd 
            // like to deactivate the ViewModel instance.
            var interval = TimeSpan.FromMinutes(5);
            Observable
                .Timer(interval, interval)
                .Subscribe(x => { /* do smth every 5m */ })
                .DisposeWith(disposables);

            // We also can observe changes of a
            // property belonging to another ViewModel,
            // so we need to unsubscribe from that
            // changes to ensure we won't have the
            // potential for a memory leak.
            /* this.WhenAnyValue()
                 .InvokeCommand()
                 .DisposeWith(disposables);*/
        });
        #endregion
    }
    private void HandleActivation()
    {
    }

    private void HandleDeactivation()
    {
    }

}
