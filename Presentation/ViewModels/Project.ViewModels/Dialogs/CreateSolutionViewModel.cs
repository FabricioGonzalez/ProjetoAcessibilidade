using System.Reactive;
using System.Reactive.Disposables;

using AppUsecases.App.Models;
using AppUsecases.App.Usecases;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.Project;

using AppViewModels.Common;
using AppViewModels.Contracts;
using AppViewModels.Dialogs.States;

using Common;

using DynamicData.Binding;

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
    private readonly GetUFList getUFList;
    private readonly IFileDialog dialogService;

    private ObservableCollectionExtended<UF> ufList;
    public ObservableCollectionExtended<UF> UFList
    {
        get => ufList;
        set => this.RaiseAndSetIfChanged(ref ufList, value, nameof(UFList));
    }

    public CreateSolutionViewModel()
    {
        solutionCreator = Locator.Current.GetService<ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel>>();

        getUFList = Locator.Current.GetService<GetUFList>();

        SolutionModel = Locator.Current.GetService<SolutionStateViewModel>();

        dialogService = Locator.Current.GetService<IFileDialog>();

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
                    CloseDialogCommand
                    .Execute()
                    .Subscribe()
                    .DisposeWith(disposables);
                }
            })
            .DisposeWith(disposables);

            UFList = new(getUFList
          .GetAllUF()
          .OrderBy(x => x.Name));
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
