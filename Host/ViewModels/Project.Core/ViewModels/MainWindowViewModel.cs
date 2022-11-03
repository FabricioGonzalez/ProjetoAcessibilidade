using System.Reactive.Disposables;

using ReactiveUI;

using Splat;

namespace Project.Core.ViewModels;
public class MainWindowViewModel : ReactiveObject, IActivatableViewModel, IScreen
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    public RoutingState Router { get; } = new RoutingState();

    public MainWindowViewModel()
    {
        var projectViewModel = new ProjectViewModel(this);
        Router.Navigate.Execute(projectViewModel);

        this.WhenActivated((CompositeDisposable disposables) =>
        {

        });
    }
}

