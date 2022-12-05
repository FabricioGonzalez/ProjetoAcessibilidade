using System.Net;
using System.Reactive;
using System.Reactive.Disposables;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.Project;

using Common;

using Project.Core.Contracts;
using Project.Core.ViewModels.Extensions;

using ReactiveUI;

using Splat;

namespace Project.Core.ViewModels.Dialogs;
public class CreateSolutionViewModel : ViewModelBase
{
    private readonly ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel> solutionCreator;

    public UIStatesStore.Solution.Models.ProjectSolutionModel SolutionModel { get; set; } = new();

    public CreateSolutionViewModel()
    {
        solutionCreator = Locator.Current.GetService<ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel>>();

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
            var dialogService = Locator.Current.GetService<IFileDialog>();

            var path = await dialogService.GetFolder();

            return path;
        });

        this.WhenActivated(disposables =>
        {
            ChooseSolutionPath.Subscribe((result) =>
                {
                    SolutionModel.FilePath = result;
                }).DisposeWith(disposables);
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
    public ReactiveCommand<Unit, Unit> CloseDialogCommand
    {
        get; set;
    }
}
