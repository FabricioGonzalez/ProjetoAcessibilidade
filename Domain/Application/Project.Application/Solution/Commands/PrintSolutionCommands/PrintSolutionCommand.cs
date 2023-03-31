using Common;

using Project.Domain.Contracts;

namespace Project.Domain.Solution.Commands.PrintSolutionCommands;
public sealed record PrintSolutionCommand(string SolutionPath) : IRequest<Resource<object>>;


public sealed class PrintSolutionCommandHandler : ICommandHandler<PrintSolutionCommand, Resource<object>>
{
    public Task<Resource<object>> Handle(PrintSolutionCommand command, CancellationToken cancellation)
    {


        return null;
    }
}
