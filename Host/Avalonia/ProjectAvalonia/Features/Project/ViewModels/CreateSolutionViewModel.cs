using System.Collections.ObjectModel;
using System.IO;
using System.Threading;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class CreateSolutionViewModel : DialogViewModelBase<ProjectSolutionModel>
{
    private ProjectSolutionModel _solutionState;
    private IMediator _mediator;
    public ProjectSolutionModel SolutionModel => _solutionState;

    public CreateSolutionViewModel(string title, ProjectSolutionModel solutionState, string caption = "")
    {
        Title = title;

        this._solutionState = solutionState;

        Title = title;
        Caption = caption;

        _mediator = Locator.Current.GetService<IMediator>();

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            UfList = new(new((await _mediator.Send(new GetAllUfQuery(), CancellationToken.None))));
        });

        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: false);
        EnableBack = false;

        var backCommandCanExecute = this
            .WhenAnyValue(property1: x => x.IsDialogOpen)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        var nextCommandCanExecute = this
            .WhenAnyValue(
                property1: x => x.IsDialogOpen,
                property2: x => x._solutionState,
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
            _solutionState.SolutionReportInfo.UF = SelectedUf;
            _solutionState.FilePath = Path.Combine(FilePath, $"{FileName}{Constants.AppProjectSolutionExtension}");
            _solutionState.FileName = FileName;

            Close(result: _solutionState);
        }
            , canExecute: nextCommandCanExecute);
        CancelCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Cancel)
            , canExecute: cancelCommandCanExecute);

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var path = await FileDialogHelper.ShowOpenFolderDialogAsync(title: "Local da Solução");

            Dispatcher.UIThread.Post(action: () =>
            {
                FilePath = path;
            });
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var path = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Logo da Empresa"
                , filterExtTypes: new[] { "png" });

            Dispatcher.UIThread.Post(action: () =>
            {

                LogoPath = path;
                _solutionState.SolutionReportInfo.LogoPath = path;
            });
        });
    }
    [AutoNotify] private string _filePath = "";
    [AutoNotify] private string _fileName = "";
    [AutoNotify] private string _logoPath = "";
    [AutoNotify] private ReadOnlyObservableCollection<UFModel> _ufList;
    [AutoNotify] private UFModel _selectedUf;


    public override MenuViewModel? ToolBar => null;
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