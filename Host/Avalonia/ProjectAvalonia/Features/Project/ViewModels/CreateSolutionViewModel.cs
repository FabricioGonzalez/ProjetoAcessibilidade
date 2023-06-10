using System.IO;
using System.Reactive;
using System.Reactive.Linq;

using Avalonia.Threading;

using Common;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.ViewModels.Dialogs.Base;

using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Domain.Contracts;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class CreateSolutionViewModel : DialogViewModelBase<ProjectSolutionModel>
{
    private readonly IMediator _mediator;
    private ProjectSolutionModel solutionState;

    public ProjectSolutionModel SolutionModel => solutionState;

    public CreateSolutionViewModel(string title, ProjectSolutionModel solutionState, string caption = "")
    {
        Title = title;
        this.solutionState = solutionState;

        Title = title;
        Caption = caption;

        _mediator = Locator.Current.GetService<IMediator>();

        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);
        EnableBack = false;

        var backCommandCanExecute = this
            .WhenAnyValue(property1: x => x.IsDialogOpen)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        var nextCommandCanExecute = this
            .WhenAnyValue(
                property1: x => x.IsDialogOpen,
                property2: x => x.solutionState,
                selector: delegate
                {
                    // This will fire validations before return canExecute value.
                    this.RaisePropertyChanged(propertyName: nameof(solutionState));

                    return IsDialogOpen;
                })
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        var cancelCommandCanExecute = this
            .WhenAnyValue(property1: x => x.IsDialogOpen)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        BackCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Back)
            , canExecute: backCommandCanExecute);
        NextCommand = ReactiveCommand.Create(execute: () => Close(result: solutionState)
            , canExecute: nextCommandCanExecute);
        CancelCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Cancel)
            , canExecute: cancelCommandCanExecute);

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var path = await FileDialogHelper.ShowOpenFolderDialogAsync(title: "Local da Solução");

            Dispatcher.UIThread.Post(action: () =>
            {
                solutionState.FilePath = Path.Combine(path,$"{solutionState.SolutionReportInfo.SolutionName}{Constants.AppProjectSolutionExtension}");
                solutionState.FileName = Path.GetFileNameWithoutExtension(path: solutionState.FilePath);

                this.RaisePropertyChanged();
            });
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var path = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Logo da Empresa"
                , filterExtTypes: new[] { "png" });

            Dispatcher.UIThread.Post(action: () =>
            {
                solutionState.SolutionReportInfo.LogoPath = path;
                this.RaisePropertyChanged();
            });
        });
    }

    public ReactiveCommand<Unit, Unit> ChooseSolutionPath
    {
        get;
    }
    public ReactiveCommand<Unit, Unit> ChooseLogoPath
    {
        get;
    }


    public string Caption
    {
        get;
    }

    public override string Title
    {
        get;
        protected set;
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    }
}