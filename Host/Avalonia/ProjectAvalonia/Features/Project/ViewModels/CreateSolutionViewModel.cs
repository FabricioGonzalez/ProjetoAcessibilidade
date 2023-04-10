using System.Reactive.Linq;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class CreateSolutionViewModel : DialogViewModelBase<SolutionStateViewModel>
{
    /*private readonly ICommandDispatcher commandDispatcher;*/
    [AutoNotify] private SolutionStateViewModel _solutionModel = new();

    public CreateSolutionViewModel(
        string title
        , string caption = ""
    )
    {
        Title = title;
        Caption = caption;

        /*commandDispatcher = Locator.Current.GetService<ICommandDispatcher>();*/

        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);
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
        NextCommand = ReactiveCommand.Create(execute: () => Close(result: SolutionModel)
            , canExecute: nextCommandCanExecute);
        CancelCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Cancel)
            , canExecute: cancelCommandCanExecute);
    }

    public override sealed string Title
    {
        get;
        protected set;
    }

    public string Caption
    {
        get;
    }
}