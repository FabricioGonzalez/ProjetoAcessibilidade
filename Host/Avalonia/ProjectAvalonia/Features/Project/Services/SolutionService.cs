﻿using System.IO;
using System.Threading.Tasks;

using Common;

using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Common.Services;
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
        var filePicker = new FilePickerService();
        var result = _solutionDatasource.ReadSolution(path);

        return result.Match(Succ: succ => succ.ToSolutionState(_locationService, filePicker)
            , Fail: fail =>
            {
                Logger.LogError(fail);

                return new SolutionState(_locationService, filePicker);
            });
    }

    public async Task CreateSolution(
        string path
        , SolutionState solution
    )
    {
        /*if (!path.Contains($"{solution.FileName}{Constants.AppProjectSolutionExtension}"))
        {
            solutionPath = Path.Combine(path, $"{solution.FileName}{Constants.AppProjectSolutionExtension}");
        }*/

        /*var solutionPath = Path.Combine(path1: path
            , path2: $"{solution.FileName}{Constants.AppProjectSolutionExtension}");*/

        /*solution.Report.SolutionName = solution.FileName;*/
        _solutionDatasource.CreateFolders(path);

        await _solutionDatasource.SaveSolution(solutionPath: path, dataToWrite: solution.ToSolutionItemRoot());

    }

    public async Task SaveSolution(
        string path
        , SolutionState solution
    )
    {
        if (!path.Contains($"{solution.FileName}{Constants.AppProjectSolutionExtension}"))
        {
            path = Path.Combine(path, $"{solution.FileName}{Constants.AppProjectSolutionExtension}");
        }

        await _solutionDatasource.SaveSolution(solutionPath: path, dataToWrite: solution.ToSolutionItemRoot());
    }
}
