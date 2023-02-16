using Common;

using MediatR;

using Project.Application.Contracts;

namespace Project.Application.Solution.Commands.PrintSolutionCommands;
public class PrintSolutionCommand : IRequest<Resource<object>>
{
    public string solutionPath
    {
        get; set;
    }
    public PrintSolutionCommand(string solutionPath)
    {
        this.solutionPath = solutionPath;
    }
}

public class PrintSolutionCommandHandler : ICommandHandler<PrintSolutionCommand, Resource<object>>
{
    public Task<Resource<object>> Handle(PrintSolutionCommand command, CancellationToken cancellation)
    {


        return null;
    }
}
