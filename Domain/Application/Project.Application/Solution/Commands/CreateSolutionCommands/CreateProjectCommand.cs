﻿using Common;

using Core.Entities.Solution;

using MediatR;

using Project.Domain.Contracts;
using Project.Domain.Solution.Contracts;

namespace Project.Domain.Solution.Commands.CreateSolutionCommands;
public class CreateSolutionCommand : IRequest<Resource<ProjectSolutionModel>>
{
    public CreateSolutionCommand(string solutionPath, ProjectSolutionModel solutionData)
    {
        SolutionPath = solutionPath;
        SolutionData = solutionData;
    }
    public string SolutionPath
    {
        get;
    }
    public ProjectSolutionModel SolutionData
    {
        get;
    }
}

public class CreateSolutionCommandHandler : ICommandHandler<CreateSolutionCommand, Resource<ProjectSolutionModel>>
{
    public ISolutionRepository repository;
    public CreateSolutionCommandHandler(ISolutionRepository solutionRepository)
    {
        repository = solutionRepository;
    }


    public async Task<Resource<ProjectSolutionModel>> Handle(CreateSolutionCommand query, CancellationToken cancellation)
    {
        if (string.IsNullOrEmpty(query.SolutionPath))
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "Caminho da solução não pode ser vázio", Data: default);
        }
        await repository.SaveSolution(query.SolutionPath, query.SolutionData);

        /*if (result is null)
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "A solução não foi encontrada", Data: default);
        }*/
        return new Resource<ProjectSolutionModel>.Success(Data: query.SolutionData);
    }
}

