using LanguageExt;
using LanguageExt.Common;
using ProjetoAcessibilidade.Core.Entities.Solution;

namespace ProjetoAcessibilidade.Domain.Solution.Contracts;

public interface ISolutionRepository
{
    public Task<Result<ProjectSolutionModel>> ReadSolution(
        Option<string> solutionPath
    );

    public Task SaveSolution(
        Option<string> solutionPath
        , Option<ProjectSolutionModel> dataToWrite
    );

    public Task SyncSolution(
        Option<string> solutionPath
        , Option<ProjectSolutionModel> dataToWrite
    );
}