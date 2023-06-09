﻿using Common.Optional;

using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Contracts;

namespace ProjetoAcessibilidade.Domain.Solution.Commands.SolutionItem;

public sealed record CreateSolutionCommand(
        string SolutionPath,
        ProjectSolutionModel SolutionData
    )
    : IRequest<Empty>;

public sealed class CreateSolutionCommandHandler
    : IHandler<CreateSolutionCommand, Empty>
{

    private readonly ISolutionRepository _repository;
    public CreateSolutionCommandHandler(ISolutionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Empty> HandleAsync(
        CreateSolutionCommand query,
        CancellationToken cancellation
    )
    {
        if (string.IsNullOrEmpty(value: query.SolutionPath))
        {
            return new Empty();
        }

        await _repository
            .SaveSolution(
                solutionPath: query.SolutionPath.ToOption(),
                dataToWrite: query.SolutionData.ToOption());

        return new Empty();
    }
}