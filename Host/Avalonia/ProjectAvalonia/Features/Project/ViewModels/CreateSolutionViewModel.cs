using System.Reactive.Linq;

using ProjectAvalonia.ViewModels.Dialogs.Base;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;
public partial class CreateSolutionViewModel : DialogViewModelBase<SolutionStateViewModel>
{
    /*private readonly ICommandDispatcher commandDispatcher;*/
    [AutoNotify] private SolutionStateViewModel _solutionModel = new();

    public CreateSolutionViewModel(string title, string caption = "")
    {
        Title = title;
        Caption = caption;

        /*commandDispatcher = Locator.Current.GetService<ICommandDispatcher>();*/

        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);
        EnableBack = false;

        var backCommandCanExecute = this
            .WhenAnyValue(x => x.IsDialogOpen)
            .ObserveOn(RxApp.MainThreadScheduler);

        var nextCommandCanExecute = this
            .WhenAnyValue(
                x => x.IsDialogOpen,
                x => x.SolutionModel,
                delegate
                {
                    // This will fire validations before return canExecute value.
                    this.RaisePropertyChanged(nameof(SolutionModel));

                    return IsDialogOpen;
                })
            .ObserveOn(RxApp.MainThreadScheduler);

        var cancelCommandCanExecute = this
            .WhenAnyValue(x => x.IsDialogOpen)
            .ObserveOn(RxApp.MainThreadScheduler);

        BackCommand = ReactiveCommand.Create(() => Close(DialogResultKind.Back), backCommandCanExecute);
        NextCommand = ReactiveCommand.Create(() => Close(result: SolutionModel), nextCommandCanExecute);
        CancelCommand = ReactiveCommand.Create(() => Close(DialogResultKind.Cancel), cancelCommandCanExecute);

    }

    public override sealed string Title
    {
        get; protected set;
    }

    public string Caption
    {
        get;
    }
}
