using System.Reactive;
using System.Reactive.Disposables;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.Project;

using AppViewModels.Common;
using AppViewModels.Dialogs.States;

using Common;


using ReactiveUI;

using Splat;

namespace AppViewModels.Dialogs;
public class CreateSolutionViewModel : ViewModelBase
{
    private readonly ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel> solutionCreator;

    public SolutionStateViewModel SolutionModel
    {
        get;
    }

    public CreateSolutionViewModel()
    {
        solutionCreator = Locator.Current.GetService<ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel>>();

        SolutionModel = Locator.Current.GetService<SolutionStateViewModel>();

        CreateSolution = ReactiveCommand.CreateFromTask(async () =>
        {
            return await solutionCreator.executeAsync(new()
            {
                FileName = SolutionModel.FileName,
                FilePath = SolutionModel.FilePath,
                ItemGroups = SolutionModel.ItemGroups.ToList(),
                ParentFolderName = SolutionModel.ParentFolderName,
                ParentFolderPath = SolutionModel.ParentFolderPath,
                reportData = SolutionModel.ReportData
            });
        });

        this.WhenActivated(disposables =>
        {
            SolutionModel.ChooseSolutionPath.Subscribe((result) =>
            {
                SolutionModel.FilePath = result;
            })
            .DisposeWith(disposables);

            SolutionModel.ChooseLogoPath.Subscribe((result) =>
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
                    CloseDialogCommand
                    .Execute()
                    .Subscribe()
                    .DisposeWith(disposables);
                }
            })
            .DisposeWith(disposables);
        });
    }

    public ReactiveCommand<Unit, Resource<ProjectSolutionModel>> CreateSolution
    {
        get; set;
    }
    public ReactiveCommand<Unit, Unit> CloseDialogCommand
    {
        get; set;
    }
}
