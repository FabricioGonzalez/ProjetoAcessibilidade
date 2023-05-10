using Common.Optional;
using ProjetoAcessibilidade.Core.Entities.Solution;

namespace ProjetoAcessibilidade.Domain.Solution.Contracts;

public interface ISolutionRepository
{
    public Task<Optional<ProjectSolutionModel>> ReadSolution(
        Optional<string> solutionPath
    );

    public Task SaveSolution(
        Optional<string> solutionPath,
        Optional<ProjectSolutionModel> dataToWrite
    );

    public Task SyncSolution(
        Optional<string> solutionPath,
        Optional<ProjectSolutionModel> dataToWrite
    );
}