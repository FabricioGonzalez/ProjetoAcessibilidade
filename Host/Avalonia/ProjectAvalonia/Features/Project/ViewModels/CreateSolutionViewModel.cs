using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.ViewModels.Dialogs.Base;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class CreateSolutionViewModel : DialogViewModelBase<SolutionState>
{
    /*private readonly ICommandDispatcher _commandDispatcher;
    private readonly SolutionStore _solutionStore;

    public ICommand ChooseLogoPath;
    public ICommand ChooseSolutionPath;

    public CreateSolutionViewModel(
        string title
        , string caption = ""
    )
    {
        Title = title;
        Caption = caption;

        _commandDispatcher = Locator.Current.GetService<ICommandDispatcher>();
        _solutionStore = Locator.Current.GetService<SolutionStore>();

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

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var path = await FileDialogHelper.ShowOpenFolderDialogAsync(title: "Local da Solução");

            Dispatcher.UIThread.Post(action: () =>
            {
                _solutionStore.CurrentOpenSolution.FilePath = path;
                _solutionStore.CurrentOpenSolution.FileName = Path.GetFileNameWithoutExtension(path: path);
            });
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var path = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Logo da Empresa"
                , filterExtTypes: new[] { "png" });

            /*Dispatcher.UIThread.Post(action: () =>
            {
                _solutionStore.CurrentOpenSolution.LogoPath = path;
            });#1#
        });
    }

    public SolutionState SolutionModel => _solutionStore.CurrentOpenSolution;
*/
    public override sealed string Title
    {
        get;
        protected set;
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    public string Caption
    {
        get;
    }
}