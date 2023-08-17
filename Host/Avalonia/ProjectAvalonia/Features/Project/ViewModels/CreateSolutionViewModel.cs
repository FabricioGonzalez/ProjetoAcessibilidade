using System.Reactive.Linq;
using ProjectAvalonia.Presentation.Interfaces.Services;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class CreateSolutionViewModel : DialogViewModelBase<(string local, SolutionState solution)>
{
    public CreateSolutionViewModel(
        string title
        , ILocationService _locationService
        , string caption = ""
    )
    {
        Title = title;

        SolutionModel = new SolutionState(_locationService);

        Title = title;
        Caption = caption;

        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: false);
        EnableBack = false;

        var backCommandCanExecute = this
            .WhenAnyValue(property1: x => x.IsDialogOpen)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        var nextCommandCanExecute = this
            .WhenAnyValue(
                property1: x => x.IsDialogOpen,
                property2: x => x.SolutionModel,
                selector: delegate
                {
                    // This will fire validations before return canExecute value.
                    this.RaisePropertyChanged(propertyName: nameof(SolutionModel));

                    return IsDialogOpen;
                })
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        var cancelCommandCanExecute = this
            .WhenAnyValue(property1: x => x.IsDialogOpen)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        BackCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Back)
            , canExecute: backCommandCanExecute);
        NextCommand = ReactiveCommand.Create(execute: () =>
            {
                Close(result: (SolutionModel.FilePath, SolutionModel));
            }
            , canExecute: nextCommandCanExecute);
        CancelCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Cancel)
            , canExecute: cancelCommandCanExecute);
    }

    public SolutionState SolutionModel
    {
        get;
    }

    public override MenuViewModel? ToolBar => null;


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