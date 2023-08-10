using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Domain.Internals;
using Domain.Solutions;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class CreateSolutionViewModel : DialogViewModelBase<(string local, ISolution solution)>
{
    /*private readonly IMediator _mediator;*/
    [AutoNotify] private string _fileName = "";
    [AutoNotify] private string _filePath = "";
    [AutoNotify] private string _logoPath = "";
    [AutoNotify] private IUf _selectedUf;
    [AutoNotify] private ReadOnlyObservableCollection<IUf> _ufList;

    public CreateSolutionViewModel(
        string title
        , string caption = ""
    )
    {
        Title = title;

        SolutionModel = new Solution();

        Title = title;
        Caption = caption;

        /*_mediator = Locator.Current.GetService<IMediator>();*/

        /*Dispatcher.UIThread.InvokeAsync(async () =>
        {
            UfList = new ReadOnlyObservableCollection<UFModel>(new ObservableCollection<UFModel>(
                await _mediator.Send(request: new GetAllUfQuery(), cancellation: CancellationToken.None)));
        });*/

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
                SolutionModel.Report.Endereco.UF = SelectedUf.Code;

                /*SolutionModel.FilePath = Path.Combine(path1: FilePath
                    , path2: $"{FileName}{Constants.AppProjectSolutionExtension}");*/

                SolutionModel.Report.SolutionName = FileName;

                Close(result: (FilePath, SolutionModel));
            }
            , canExecute: nextCommandCanExecute);
        CancelCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Cancel)
            , canExecute: cancelCommandCanExecute);

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            /*var path = await FileDialogHelper.ShowOpenFolderDialogAsync(title: "Local da Solução");

            Dispatcher.UIThread.Post(action: () =>
            {
                FilePath = path;
            });*/
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            /*var path = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Logo da Empresa"
                , filterExtTypes: new[] { "png" });

            Dispatcher.UIThread.Post(action: () =>
            {
                LogoPath = path;
                SolutionModel.Report.LogoPath = path;
            });*/
        });
    }

    public ISolution SolutionModel
    {
        get;
    }


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