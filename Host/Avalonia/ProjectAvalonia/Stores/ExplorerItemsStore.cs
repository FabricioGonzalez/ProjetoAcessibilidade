using System.Threading;
using System.Windows.Input;
using ProjetoAcessibilidade.Domain.Contracts;

namespace ProjectAvalonia.Stores;

public class ExplorerItemsStore
{
    private readonly CancellationTokenSource _cancellationToken;
    private readonly IMediator _mediator;
    private readonly SolutionStore _solutionStore;

    public ExplorerItemsStore(SolutionStore solutionStore, IMediator mediator)
    {
        _solutionStore = solutionStore;
        _mediator = mediator;

        _cancellationToken = new CancellationTokenSource();
    }

    public ICommand ExcludeFileCommand
    {
        get;
        set;
    }

    public ICommand RenameFileCommand
    {
        get;
        set;
    }

    public ICommand ExcludeFolderCommand
    {
        get;
        set;
    }

    public ICommand AddProjectItemCommand
    {
        get;
        set;
    }

    public ICommand CreateFolderCommand
    {
        get;
        set;
    }

    public ICommand CommitFolderCommand
    {
        get;
        set;
    }

    public void CancelTask()
    {
        if (_cancellationToken.TryReset())
        {
            _cancellationToken.Cancel();
        }
    }
}