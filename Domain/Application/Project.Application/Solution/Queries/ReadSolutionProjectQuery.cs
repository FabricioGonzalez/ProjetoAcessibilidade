using Common;
using Common.Optional;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Solution.Queries;

public sealed record ReadSolutionProjectQuery(
    string SolutionPath
) : IRequest<Resource<ProjectSolutionModel>>;

public sealed class ReadSolutionProjectQueryHandler
    : IHandler<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>
{
    /*public ISolutionRepository repository;

    public ReadSolutionProjectQueryHandler(
        ISolutionRepository solutionRepository
    )
    {
        repository = solutionRepository;
    }*/


    public async Task<Resource<ProjectSolutionModel>> HandleAsync(
        ReadSolutionProjectQuery query,
        CancellationToken cancellation
    ) =>
        await Locator.Current.GetService<ISolutionRepository>()
            .ToOption()
            .Map<Task<Resource<ProjectSolutionModel>>>(
                async instance =>
                {
                    return await query.SolutionPath
                        .ToOption()
                        .Map<Task<Resource<ProjectSolutionModel>>>(
                            async path =>
                            {
                                var result = await instance.ReadSolution(solutionPath: path.ToOption());

                                return result.Map<Resource<ProjectSolutionModel>>(
                                        data => new Resource<ProjectSolutionModel>.Success(Data: data))
                                    .Reduce(
                                        () =>
                                            new Resource<ProjectSolutionModel>.Error(
                                                Message: "A solução não foi encontrada",
                                                Data: default));
                            })
                        .Reduce(
                            () => new Task<Resource<ProjectSolutionModel>>(
                                () => new Resource<ProjectSolutionModel>.Error(
                                    Message: "Caminho da solução não pode ser vázio",
                                    Data: default)));
                })
            .Reduce(
                () =>
                    new Task<Resource<ProjectSolutionModel>>(
                        () => new Resource<ProjectSolutionModel>.Error(
                            Message: $"A Instancia {nameof(ISolutionRepository)} não foi encontrada",
                            Data: default))
            );
}