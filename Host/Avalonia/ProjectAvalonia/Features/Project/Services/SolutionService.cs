using System.IO;
using System.Threading.Tasks;

using Common;

using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Features.Project.Mappings;
using ProjectAvalonia.Presentation.Interfaces.Services;
using ProjectAvalonia.Presentation.States;

using XmlDatasource.Solution;

namespace ProjectAvalonia.Features.Project.Services;

public class SolutionService
{
    private readonly ILocationService _locationService;
    private readonly SolutionDatasourceImpl _solutionDatasource;

    public SolutionService(
        SolutionDatasourceImpl datasourceImpl
        , ILocationService locationService
    )
    {
        _solutionDatasource = datasourceImpl;
        _locationService = locationService;
    }

    public SolutionState GetSolution(
        string path
    )
    {
        var result = _solutionDatasource.ReadSolution(path);

        return result.Match(Succ: succ => succ.ToSolutionState(_locationService)
            , Fail: fail =>
            {
                Logger.LogError(fail);

                return new SolutionState(_locationService);
            });
    }

    public async Task CreateSolution(
        string path
        , SolutionState solution
    )
    {
        var solutionPath = Path.Combine(path1: path
            , path2: $"{solution.FileName}{Constants.AppProjectSolutionExtension}");

        solution.Report.SolutionName = solution.FileName;

        await _solutionDatasource.SaveSolution(solutionPath: solutionPath, dataToWrite: solution.ToSolutionItemRoot());
        _solutionDatasource.CreateFolders(path);
    }

    public async Task SaveSolution(
        string path
        , SolutionState solution
    ) =>
        await _solutionDatasource.SaveSolution(solutionPath: path, dataToWrite: solution.ToSolutionItemRoot());
}
