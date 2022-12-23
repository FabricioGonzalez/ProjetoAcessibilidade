using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using AppViewModels.Common;
using AppViewModels.Contracts;
using AppViewModels.Dialogs;
using AppViewModels.Dialogs.States;
using AppViewModels.Interactions.Main;
using AppViewModels.Interactions.Project;
using AppViewModels.Main.States;
using AppViewModels.Project;
using AppViewModels.System;
using AppViewModels.TemplateEditing;
using AppViewModels.TemplateRules;

using Project.Application.App.Contracts;

using ReactiveUI;

using Splat;

namespace AppViewModels.Main;
public class MainViewModel : ViewModelBase, IActivatableViewModel, IScreen
{
    public RoutingState Router { get; } = new RoutingState();

    public AppMessageState appMessage
    {
        get; set;
    } = new();

    public INotificationMessageManagerService NotificationMessageManagerService
    {
        get;
    }

    public SolutionStateViewModel SolutionModel
    {
        get;
    }
    public MainViewModel()
    {
        NotificationMessageManagerService ??= Locator.Current.GetService<INotificationMessageManagerService>();

        /*projectState ??= Locator.Current.GetService<IAppObservable<ProjectModel>>();*/

        SolutionModel = Locator.Current.GetService<SolutionStateViewModel>();

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

        GoToSetting = ReactiveCommand.Create(() =>
        {
            var settings = Locator.Current.GetService<SettingsViewModel>();

            if (settings is not null)
            {
                settings.HostScreen = this;

                Router.Navigate.Execute(settings);
            }
        });

        this.WhenActivated((disposables) =>
        {
            OpenProjectCommand.Subscribe(solutionPath =>
            {
                var result = ProjectInteractions
                     .SelectedProjectPath
                      .Handle(solutionPath)
                      .Subscribe(x =>
                      {
                      })
                       .DisposeWith(disposables);
            })
            .DisposeWith(disposables);

            AppInterations
                      .MessageQueue
                      .RegisterHandler(interaction =>
                      {
                          var message = interaction.Input;

                          switch (message.Type)
                          {
                              case App.Core.Entities.App.MessageType.Debug:

                                  NotificationMessageManagerService.ShowDebug(message.Message);
                                  break;
                              case App.Core.Entities.App.MessageType.Info:

                                  NotificationMessageManagerService.ShowInfo(message.Message);
                                  break;
                              case App.Core.Entities.App.MessageType.Error:

                                  NotificationMessageManagerService.ShowError(message.Message);
                                  break;
                              case App.Core.Entities.App.MessageType.Warning:

                                  NotificationMessageManagerService.ShowWarning(message.Message);
                                  break;
                              default:
                                  NotificationMessageManagerService.ShowError("Tipo não definido");
                                  break;
                          };

                          interaction.SetOutput(interaction.Input);

                      })
                      .DisposeWith(disposables);


            CreateProjectCommand.Subscribe(solutionPath =>
            {
                ProjectInteractions
               .SelectedProjectPath
               .Handle(solutionPath)
               .Subscribe(x =>
                 {
                 })
               .DisposeWith(disposables); ;
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
    public ReactiveCommand<Unit, Unit> GoToSetting
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

    public Interaction<CreateSolutionViewModel, MainViewModel?> ShowSolutionCreateDialog
    {
        get; private set;
    } = new Interaction<CreateSolutionViewModel, MainViewModel?>();

}
