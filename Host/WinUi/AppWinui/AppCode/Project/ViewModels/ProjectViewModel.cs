using System.Reactive;
using System.Reactive.Disposables;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities;

using AppWinui.AppCode.AppUtils.Contracts.Services;
using AppWinui.AppCode.AppUtils.ViewModels;
using AppWinui.AppCode.Project.DTOs;
using AppWinui.AppCode.Project.States;
using AppWinui.AppCode.TemplateEditing.ViewModels;
using AppWinui.AppCode.ValidationRules.ViewModels;

using Common;

using ReactiveUI;

namespace AppWinui.AppCode.Project.ViewModels;
public class ProjectViewModel : ReactiveObject, IActivatableViewModel
{
    public ApplicationViewModel AppViewModel
    {
        get; set;
    }
    public ExplorerViewViewModel ExplorerViewModel
    {
        get; set;
    }
    public RecentOpenedViewModel RecentOpenedViewModel
    {
        get; set;
    }
    public ReportDataState? Report
    {
        get; set;
    }

    private bool _isProjectOpened = false;
    public bool IsProjectOpened
    {
        get => _isProjectOpened;
        set => this.RaiseAndSetIfChanged(
            ref _isProjectOpened,
            value,
            nameof(IsProjectOpened));
    }

    private readonly IQueryUsecase<ProjectSolutionModel> getSolutionUsecase;
    private readonly ICommandUsecase<ProjectSolutionModel> createSolutionUsecase;
    private readonly INavigationService navigationService;

    public ViewModelActivator Activator
    {
        get;
    }

    public ProjectViewModel(
        IQueryUsecase<ProjectSolutionModel> getSolutionUsecase,
        ICommandUsecase<ProjectSolutionModel> createSolutionUsecase,
        INavigationService navigationService
        )
    {
        this.getSolutionUsecase = getSolutionUsecase;
        this.createSolutionUsecase = createSolutionUsecase;
        this.navigationService = navigationService;

        ExplorerViewModel = App.GetService<ExplorerViewViewModel>();
        AppViewModel = App.GetService<ApplicationViewModel>();
        RecentOpenedViewModel = App.GetService<RecentOpenedViewModel>();

        Activator = new ViewModelActivator();

        this.WhenActivated(d =>
        {
            OpenProjectCommand = ReactiveCommand
           .CreateFromTask(() => this.getSolutionUsecase.executeAsync());

            OpenProjectCommand.Subscribe(x =>
            {
                if (x is Resource<ProjectSolutionModel>.Error error)
                {

                }
                if (x is Resource<ProjectSolutionModel>.IsLoading isLoading)
                {

                }
                if (x is Resource<ProjectSolutionModel>.Success success)
                {
                    IsProjectOpened = true;
                    ExplorerViewModel.ProjectRootPath = success.Data.ParentFolderPath;

                    Report = success.Data.reportData.ToReportDataState();
                }
            })
                .DisposeWith(d);
        });

        GoToTemplatesCommand = ReactiveCommand.Create(() =>
        {
            this.navigationService.NavigateTo(typeof(TemplateEditViewModel).FullName);
        });

        GoToRulesCommand = ReactiveCommand.Create(() =>
        {
            this.navigationService.NavigateTo(typeof(ValidationRulesViewModel).FullName);
        });

        GoToPrintCommand = ReactiveCommand.Create(() =>
        {
            /*this.navigationService.NavigateTo(typeof(Print).FullName);*/
        });

        GoToHelpCommand = ReactiveCommand.Create(() =>
        {
            /*this.navigationService.NavigateTo(typeof(TemplateEditViewModel).FullName);*/
        });


        //CreateProjectCommand = ReactiveCommand
        //    .CreateFromTask(async () => await this.createSolutionUsecase.executeAsync(null));

    }
    public ReactiveCommand<Unit, Resource<ProjectSolutionModel>> OpenProjectCommand
    {
        get; private set;
    }
    public ReactiveCommand<Unit, Resource<ProjectSolutionModel>> CreateProjectCommand
    {
        get; private set;
    }

    public ReactiveCommand<Unit, Unit> GoToTemplatesCommand
    {
        get; private set;
    }

    public ReactiveCommand<Unit, Unit> GoToRulesCommand
    {
        get; private set;
    }

    public ReactiveCommand<Unit, Unit> GoToPrintCommand
    {
        get; private set;
    }

    public ReactiveCommand<Unit, Unit> GoToHelpCommand
    {
        get; private set;
    }

    public void Dispose()
    {
    }
}
