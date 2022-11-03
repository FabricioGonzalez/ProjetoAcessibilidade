using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using AppUsecases.App.Contracts.Services;
using AppUsecases.Contracts.Repositories;
using AppUsecases.Project.Entities.Project;

using Windows.Storage;

namespace WindowsLocalRepository.FileRepository.Repository.InternalAppFiles;
public class ReadUserProjectSolutionFileRepository : IReadContract<ProjectSolutionModel>
{
 /*   private readonly IFilePickerService filePickerService;
    public ReadUserProjectSolutionFileRepository(IFilePickerService filePickerService)
    {
        this.filePickerService = filePickerService;
    }*/
    public async Task<ProjectSolutionModel> ReadAsync()
    {/*
        var path = await filePickerService.GetFile(new string[] { ".prja" });

        if (path is not null)
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            if (file is not null)
            {
                var folder = await file.GetParentAsync();

                var solution = new ProjectSolutionModel()
                {
                    FileName = file.Name,
                    FilePath = file.Path,
                    ParentFolderName = folder.Name,
                    ParentFolderPath = folder.Path,
                    reportData = new()
                };

                using var reader = new StreamReader(await file.OpenStreamForReadAsync());

                if (await reader.ReadToEndAsync() is string data && data.Length > 0)
                {
                    var resultData = JsonSerializer.Deserialize<ReportDataModel>(data) ?? null;

                    if (resultData is not null)
                        solution.reportData = resultData;

                    folder = null;
                    file = null;
                }

                return solution;
            }
        }*/
        return null;
    }
}
