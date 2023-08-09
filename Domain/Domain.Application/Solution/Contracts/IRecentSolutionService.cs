using Domain.Solutions;
using LanguageExt.Common;

namespace Application.Solution.Contracts;

public interface IRecentSolutionService
{
    public Task<Result<IEnumerable<IRecentSolution>>> GetAllRecentSolution();
    public Task<Result<IEnumerable<IRecentSolution>>> SaveRecentSolutionEntry();
}