using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;

using Project.Core.Contracts;

using ReactiveUI;

using Splat;

using UIStatesStore.Contracts;
using UIStatesStore.Project.Models;
using UIStatesStore.Project.Observable;

namespace Project.Core.ViewModels;
public class MainWindowViewModel : ViewModelBase, IActivatableViewModel, IScreen
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    public RoutingState Router { get; } = new RoutingState();

    /*    public ExplorerComponentViewModel explorerViewModel { get; }*/

    readonly IAppObservable<ProjectModel> projectState;

    public MainWindowViewModel()
    {
        projectState = Locator.Current.GetService<IAppObservable<ProjectModel>>();

        var projectViewModel = new ProjectViewModel(this);
        Router.Navigate.Execute(projectViewModel);

        OpenProjectCommand = ReactiveCommand.CreateFromTask<Unit, string>(async (Unit) =>
        {
            var dialog = Locator.Current.GetService<IFileDialog>();

            if (dialog is not null)
            {
                var result = await dialog.GetFile();
                return result;
            }

            return "";
        });

        CreateProjectCommand = ReactiveCommand.CreateFromTask<Unit, string>(async (Unit) =>
        {
            var dialog = Locator.Current.GetService<IFileDialog>();

            if (dialog is not null)
            {
                var result = await dialog.SaveFile();
                return result;
            }

            return "";
        });

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            OpenProjectCommand.Subscribe(x =>
            {
                projectState.Send(new(x));
            })
            .DisposeWith(disposables);

            CreateProjectCommand.Subscribe(x =>
            {
                projectState.Send(new(x));
            })
            .DisposeWith(disposables);
        });
    }

    public ReactiveCommand<Unit, string> OpenProjectCommand
    {
        get; private set;
    }   
    public ReactiveCommand<Unit, string> CreateProjectCommand
    {
        get; private set;
    }
}

