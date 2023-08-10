using Application.Solution.Contracts;
using Domain.Solutions;

namespace Application.Solution.Services;

public sealed class SolutionManipulationService : ISolutionService
{
    private readonly ISolutionRepository _solutionRepository;

    public SolutionManipulationService(
        ISolutionRepository solutionRepository
    )
    {
        _solutionRepository = solutionRepository;
    }

    public async Task<ISolution> OpenSolution(
        string path
    ) =>
        await _solutionRepository.OpenSolution(path);

    public async Task SaveSolution(
        string path
        , ISolution solutionToSave
    ) =>
        await _solutionRepository.SaveSolution(path: path, solutionToSave: solutionToSave);
}