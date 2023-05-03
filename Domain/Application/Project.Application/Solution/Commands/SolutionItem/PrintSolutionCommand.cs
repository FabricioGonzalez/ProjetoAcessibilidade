using Common;
using ProjetoAcessibilidade.Domain.Contracts;

namespace ProjetoAcessibilidade.Domain.Solution.Commands.SolutionItem;

public sealed record PrintSolutionCommand(
    string SolutionPath
) : IRequest<Resource<object>>;

public sealed class PrintSolutionCommandHandler : IHandler<PrintSolutionCommand, Resource<object>>
{
    public Task<Resource<object>> HandleAsync(
        PrintSolutionCommand command
        , CancellationToken cancellation
    ) => null;
}