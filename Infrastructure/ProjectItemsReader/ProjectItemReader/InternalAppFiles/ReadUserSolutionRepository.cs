using System.Text.Json;

using AppUsecases.App.Contracts.Repositories;
using AppUsecases.Project.Entities.Project;

namespace ProjectItemReader.InternalAppFiles;
public class ReadUserSolutionRepository : IReadContract<ProjectSolutionModel>
{
 /*   private readonly IFilePickerService filePickerService;
    public ReadUserProjectSolutionFileRepository(IFilePickerService filePickerService)
    {
        this.filePickerService = filePickerService;
    }*/
    public async Task<ProjectSolutionModel> ReadAsync(string path)
    {
        /*var path = await filePickerService.GetFile(new string[] { ".prja" });*/

        if (path is not null)
        {
            var splittedPath = path.Split(Path.DirectorySeparatorChar);
            var file = Directory.GetFiles(string.Join(Path.DirectorySeparatorChar
                , splittedPath[..(splittedPath.Length - 1)]))
                .FirstOrDefault(file => file.Equals(splittedPath[^1]));

            if (file is not null)
            {
                var folder = Directory
                    .GetDirectories(string.Join(Path.DirectorySeparatorChar, file.Split(Path.DirectorySeparatorChar)
                    [..(file.Split(Path.DirectorySeparatorChar).Length - 1)]))
                    .FirstOrDefault(folder => folder.Equals(
                        string.Join(Path.DirectorySeparatorChar, file.Split(Path.DirectorySeparatorChar)[..(file.Split(Path.DirectorySeparatorChar).Length - 1)]))
                    );

                var solution = new ProjectSolutionModel()
                {
                    FileName = file.Split(Path.DirectorySeparatorChar)[^1],
                    FilePath = file,
                    ParentFolderName = folder.Split(Path.DirectorySeparatorChar)[^1],
                    ParentFolderPath = folder,
                    reportData = new()
                };

                using var reader = new StreamReader(file);

                if (await reader.ReadToEndAsync() is string data && data.Length > 0)
                {
                    var resultData = JsonSerializer.Deserialize<ReportDataModel>(data) ?? null;

                    if (resultData is not null)
                    {
                        solution.reportData = resultData;
                    }

                    folder = null;
                    file = null;
                }

                return solution;
            }
        }
        return null;
    }
}
