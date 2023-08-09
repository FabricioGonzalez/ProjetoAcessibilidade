using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using Avalonia.Threading;
using Core.Entities.App;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Presentation.Interfaces;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Domain.App.Queries.UF;
using ProjetoAcessibilidade.Domain.Contracts;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels.EditingItemBody;

public partial class SolutionItemBody
    : ReactiveObject
        , ISolutionEditingBody
{
    private readonly IMediator _mediator;
    [AutoNotify] private string _fileName = "";
    [AutoNotify] private string _filePath = "";
    [AutoNotify] private string _logoPath = "";
    [AutoNotify] private UFModel _selectedUf;
    [AutoNotify] private ReadOnlyObservableCollection<UFModel> _ufList;

    public SolutionItemBody(
        ProjectSolutionModel solutionState
    )
    {
        SolutionModel = solutionState;

        _mediator = Locator.Current.GetService<IMediator>();

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            UfList = new ReadOnlyObservableCollection<UFModel>(
                new ObservableCollection<UFModel>(await _mediator.Send(request: new GetAllUfQuery()
                    , cancellation: CancellationToken.None)));
        });

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var path = await FileDialogHelper.ShowOpenFolderDialogAsync("Local da Solução");

            Dispatcher.UIThread.Post(() =>
            {
                FilePath = path;
            });
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var path = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Logo da Empresa"
                , filterExtTypes: new[] { "png" });

            Dispatcher.UIThread.Post(() =>
            {
                LogoPath = path;
                SolutionModel.SolutionReportInfo.LogoPath = path;
            });
        });
    }

    public ProjectSolutionModel SolutionModel
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
}