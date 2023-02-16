using System.Reactive;
using System.Reactive.Disposables;

using AppViewModels.Common;
using AppViewModels.Dialogs.States;

using Common;

using Core.Entities.Solution;

using Project.Application.Contracts;

using ReactiveUI;

using Splat;

namespace AppViewModels.Dialogs;
public class CreateSolutionViewModel : ViewModelBase
{
    private readonly ICommandDispatcher commandDispatcher;

    public SolutionStateViewModel SolutionModel
    {
        get;
    }

    public CreateSolutionViewModel()
    {
        commandDispatcher = Locator.Current.GetService<ICommandDispatcher>();

        SolutionModel = Locator.Current.GetService<SolutionStateViewModel>();

        CreateSolution = ReactiveCommand.CreateFromTask<Resource<ProjectSolutionModel>>(async () =>
        {
            /*  return await commandDispatcher.Dispatch(new()
              {
                  FileName = SolutionModel.FileName,
                  FilePath = SolutionModel.FilePath,
                  ItemGroups = SolutionModel.ItemGroups.ToList(),
                  ParentFolderName = SolutionModel.ParentFolderName,
                  ParentFolderPath = SolutionModel.ParentFolderPath,
                  reportData = SolutionModel.ReportData
              });*/
            return new Resource<ProjectSolutionModel>.Success(new());
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
                    .Subscribe();
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
