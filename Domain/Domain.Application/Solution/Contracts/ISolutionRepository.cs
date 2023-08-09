using Domain.Solutions;

namespace Application.Solution.Contracts;

public interface ISolutionRepository
{
    public Task<ISolution> OpenSolution(
        string path
    );


    public Task SaveSolution(
        string path
        , ISolution solutionToSave
    );
}