using System.Reactive;
using System.Reactive.Disposables;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities;

using AppWinui.AppCode.AppUtils.ViewModels;
using AppWinui.AppCode.Project.DTOs;
using AppWinui.AppCode.Project.States;

using Common;

using ReactiveUI;

namespace AppWinui.AppCode.Project.ViewModels;
public class ProjectViewModel : ReactiveObject,IActivatableViewModel
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

    private readonly IQueryUsecase<object, ProjectSolutionModel> getSolutionUsecase;
    private readonly ICommandUsecase<object, ProjectSolutionModel> createSolutionUsecase;

    public ReactiveCommand<Unit, Resource<ProjectSolutionModel>> OpenProjectCommand
    {
        get; private set;
    }
    public ReactiveCommand<Unit, Resource<ProjectSolutionModel>> CreateProjectCommand
    {
        get; private set;
    }

    public ViewModelActivator Activator { get; }

    public ProjectViewModel(IQueryUsecase<object, ProjectSolutionModel> getSolutionUsecase,
        ICommandUsecase<object, ProjectSolutionModel> createSolutionUsecase)
    {
        this.getSolutionUsecase = getSolutionUsecase;
        this.createSolutionUsecase = createSolutionUsecase;

        ExplorerViewModel = App.GetService<ExplorerViewViewModel>();
        AppViewModel = App.GetService<ApplicationViewModel>();
        RecentOpenedViewModel = App.GetService<RecentOpenedViewModel>();

        Activator = new ViewModelActivator();

        this.WhenActivated(d =>
        {
            OpenProjectCommand = ReactiveCommand
           .CreateFromTask(() => this.getSolutionUsecase.executeAsync(null));

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

       
        //CreateProjectCommand = ReactiveCommand
        //    .CreateFromTask(async () => await this.createSolutionUsecase.executeAsync(null));
      
    }

    public void Dispose() { }
}
