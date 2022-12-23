using System.Reactive;
using System.Reactive.Disposables;

using AppUsecases.App.Contracts.Usecases;
using AppUsecases.App.Models;
using AppUsecases.App.Usecases;
using AppUsecases.Project.Entities.Project;

using Common;

using Project.Core.Contracts;
using Project.Core.ViewModels.Extensions;

using ReactiveUI;

using Splat;

using UIStatesStore.Contracts;
using UIStatesStore.Project.Models;

namespace Project.Core.ViewModels.Dialogs;
public class CreateSolutionViewModel : ViewModelBase
{
    private readonly ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel> solutionCreator;

    public UIStatesStore.Solution.Models.ProjectSolutionModel SolutionModel { get; set; } = new();
    private readonly IAppObservable<ProjectModel> projectPath;
    private readonly IAppObservable<UIStatesStore.Solution.Models.ProjectSolutionModel> solutionObserver;
    private readonly GetUFList getUFList;
    private readonly IFileDialog dialogService;

    public List<UF> UFList
    {
        get;set;
    } 

    public CreateSolutionViewModel()
    {
        solutionCreator = Locator.Current.GetService<ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel>>();
        projectPath = Locator.Current.GetService<IAppObservable<ProjectModel>>();
        solutionObserver = Locator.Current.GetService<IAppObservable<UIStatesStore.Solution.Models.ProjectSolutionModel>>();
        getUFList = Locator.Current.GetService<GetUFList>();

        dialogService = Locator.Current.GetService<IFileDialog>();

        CreateSolution = ReactiveCommand.CreateFromTask(async () =>
        {
            var result = await solutionCreator.executeAsync(new()
            {
                FileName = SolutionModel.FileName,
                FilePath = SolutionModel.FilePath,
                ItemGroups = SolutionModel.ItemGroups.ToList(),
                ParentFolderName = SolutionModel.ParentFolderName,
                ParentFolderPath = SolutionModel.ParentFolderPath,
                reportData = SolutionModel.ReportData
            });

            return result;
        });

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var path = await dialogService.GetFolder();

            return path;
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var path = await dialogService.GetFolder();

            return path;
        });

        this.WhenActivated(disposables =>
        {
            ChooseSolutionPath.Subscribe((result) =>
                {
                    SolutionModel.FilePath = result;
                })
            .DisposeWith(disposables);

            ChooseLogoPath.Subscribe((result) =>
            {
                SolutionModel.ReportData.LogoPath = result;
            })
           .DisposeWith(disposables);

            CreateSolution.Subscribe((result) =>
            {
                result
                .OnError(out var data, out var message)
                .OnLoading(out data, out var isLoading)
                .OnSuccess(out data);

                if (data is not null)
                {
                    projectPath.Send(new(Path.Combine(data.FilePath, data.FileName)));

                    solutionObserver.Send(new()
                    {
                        ReportData = data.reportData,
                        FileName = data.FileName,
                        FilePath = data.FilePath,
                        ItemGroups = new(data.ItemGroups),
                    });
                    
                    CloseDialogCommand
                    .Execute()
                    .Subscribe()
                    .DisposeWith(disposables);
                }
            })
            .DisposeWith(disposables);

            UFList = getUFList.GetAllUF();
        });

    }


    public ReactiveCommand<Unit, Resource<ProjectSolutionModel>> CreateSolution
    {
        get; set;
    }
    public ReactiveCommand<Unit, string> ChooseSolutionPath
    {
        get; set;
    }   
    public ReactiveCommand<Unit, string> ChooseLogoPath
    {
        get; set;
    }
    public ReactiveCommand<Unit, Unit> CloseDialogCommand
    {
        get; set;
    }
}
