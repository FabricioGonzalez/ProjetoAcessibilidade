using AppRepositories.Solution.Dto;
using LanguageExt.Common;

namespace AppRepositories.Solution.Contracts;

public interface ISolutionDatasource
{
    Task SaveSolution(
        string solutionPath
        , SolutionItem dataToWrite
    );

    public Result<SolutionItem> ReadSolution(
        string solutionPath
    );
}