using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;

using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.Models;
using ProjectAvalonia.Presentation.States;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.EditingItemBody;

public partial class SolutionItemBody
    : ReactiveObject
        , ISolutionEditingBody
{
    /*private readonly IMediator _mediator;*/
    [AutoNotify] private string _fileName = "";
    [AutoNotify] private string _filePath = "";
    [AutoNotify] private string _logoPath = "";
    [AutoNotify] private Uf _selectedUf;
    [AutoNotify] private ReadOnlyObservableCollection<Uf> _ufList;
    private CompositeDisposable disposables = new();
    public SolutionItemBody(
        SolutionState solutionState
    )
    {
        SolutionModel = solutionState;

        /*_mediator = Locator.Current.GetService<IMediator>();*/

        /*Dispatcher.UIThread.InvokeAsync(async () =>
        {
            UfList = new ReadOnlyObservableCollection<UFModel>(
                new ObservableCollection<UFModel>(await _mediator.Send(request: new GetAllUfQuery()
                    , cancellation: CancellationToken.None)));
        });*/

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(async () =>
        {
            /*var path = await FileDialogHelper.ShowOpenFolderDialogAsync("Local da Solução");

            Dispatcher.UIThread.Post(() =>
            {
                FilePath = path;
            });*/
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(async () =>
        {
            /*var path = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Logo da Empresa"
                , filterExtTypes: new[] { "png" });

            Dispatcher.UIThread.Post(() =>
            {
                LogoPath = path;
                SolutionModel.LogoPath = path;
            });*/
        });
    }

    public SolutionState SolutionModel
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ChooseSolutionPath
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ChooseLogoPath
    {
        get;
    }

    public void Dispose()
    {
        disposables.Dispose();
    }
}