using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using Project.Core.Contracts;
using Project.Core.ViewModels.Dialogs;
using Project.Core.ViewModels.Extensions;
using Project.Core.ViewModels.Project;
using Project.Core.ViewModels.TemplateEditing;
using Project.Core.ViewModels.TemplateRules;

using ReactiveUI;

using Splat;

using UIStatesStore.Contracts;
using UIStatesStore.Project.Models;

namespace Project.Core.ViewModels.Main;
public class MainWindowViewModel : ViewModelBase, IActivatableViewModel, IScreen
{
    public RoutingState Router { get; } = new RoutingState();

    private readonly IAppObservable<ProjectModel> projectState;

    public void SetProjectPath(string projectPath)
    {
        projectState.Send(new(projectPath));
    }

    public MainWindowViewModel()
    {
        projectState ??= Locator.Current.GetService<IAppObservable<ProjectModel>>();

        Router.Navigate.Execute(Locator.Current.GetService<ProjectViewModel>());

        OpenProjectCommand = ReactiveCommand.CreateFromTask<Unit, string>(async (Unit) =>
        {
            ReturnToProject();

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
            /* ReturnToProject();

             var dialog = Locator.Current.GetService<IFileDialog>();

             if (dialog is not null)
             {
                 var result = await dialog.SaveFile();

                 return result;
             }
 */
            CreateSolution();
            return "";
        });

        GoToRulesEditing = ReactiveCommand.Create(() =>
        {
            var rulesEditing = Locator.Current.GetService<TemplateRulesViewModel>();

            if (rulesEditing is not null)
            {
                rulesEditing.HostScreen = this;

                Router.Navigate.Execute(rulesEditing);
            }
        });
       
        GoToTemplateEditing = ReactiveCommand.Create(() =>
        {
            var templateEditing = Locator.Current.GetService<TemplateEditingViewModel>();

            if (templateEditing is not null)
            {
                templateEditing.HostScreen = this;

                Router.Navigate.Execute(templateEditing);
            }
        });

        ShowSolutionCreateDialog = new Interaction<CreateSolutionViewModel, MainWindowViewModel?>();

        this.WhenActivated((disposables) =>
        {
            OpenProjectCommand.Subscribe(solutionPath =>
            {
                SetProjectPath(solutionPath);
            })
            .DisposeWith(disposables);


            CreateProjectCommand.Subscribe(solutionPath =>
            {
                SetProjectPath(solutionPath);
             
            })
            .DisposeWith(disposables);

        });
    }

    private async void CreateSolution()
    {
        var store = new CreateSolutionViewModel();

        var result = await ShowSolutionCreateDialog.Handle(store);


    }

    private void ReturnToProject()
    {
        if (Router.CurrentViewModel is not ProjectViewModel)
        {
            var projectEditing = Locator.Current.GetService<ProjectViewModel>();

            if (projectEditing is not null)
            {
                projectEditing.HostScreen = this;

                Router.Navigate.Execute(projectEditing);
            }
        }
    }

    public ReactiveCommand<Unit, string> OpenProjectCommand
    {
        get; private set;
    }
    public ReactiveCommand<Unit, string> CreateProjectCommand
    {
        get; private set;
    }
    public ReactiveCommand<Unit, Unit> GoToTemplateEditing
    {
        get; private set;
    }
    public ReactiveCommand<Unit, Unit> GoToRulesEditing
    {
        get; private set;
    }    
    public ReactiveCommand<Unit, Unit> CloseAppCommand
    {
        get; set;
    }

    public Interaction<CreateSolutionViewModel, MainWindowViewModel?> ShowSolutionCreateDialog
    {
        get; private set;
    }

}

