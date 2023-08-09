using Application.Solution.Contracts;
using Domain.Solutions;
using LanguageExt.Common;

namespace Application.Solution.Services;

public sealed class RecentSolutionService : IRecentSolutionService
{
    public Task<Result<IEnumerable<IRecentSolution>>> GetAllRecentSolution() => throw new NotImplementedException();

    public Task<Result<IEnumerable<IRecentSolution>>> SaveRecentSolutionEntry() => throw new NotImplementedException();
}