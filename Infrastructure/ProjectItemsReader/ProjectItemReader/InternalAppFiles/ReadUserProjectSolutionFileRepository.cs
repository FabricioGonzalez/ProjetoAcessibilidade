﻿using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using AppUsecases.App.Contracts.Services;
using AppUsecases.Contracts.Repositories;
using AppUsecases.Project.Entities.Project;

namespace ProjectItemReader.InternalAppFiles;
public class ReadUserProjectSolutionFileRepository : IReadContract<ProjectSolutionModel>
{
    private readonly IFilePickerService filePickerService;
    public ReadUserProjectSolutionFileRepository(IFilePickerService filePickerService)
    {
        this.filePickerService = filePickerService;
    }
    public async Task<ProjectSolutionModel> ReadAsync()
    {
        var path = await filePickerService.GetFile(new string[] { ".prja" });

        if (path is not null)
        {
            var file = Directory.GetFiles(string.Join(Path.DirectorySeparatorChar
                , path.Split(Path.DirectorySeparatorChar)[..(path.Split(Path.DirectorySeparatorChar).Length - 1)]))
                .FirstOrDefault(file => file.Equals(path.Split(Path.DirectorySeparatorChar)[path.Split(Path.DirectorySeparatorChar).Length - 1]));

            if (file is not null)
            {
                var folder = Directory
                    .GetDirectories(string.Join(Path.DirectorySeparatorChar, file.Split(Path.DirectorySeparatorChar)[..(file.Split(Path.DirectorySeparatorChar).Length - 1)]))
                    .FirstOrDefault(folder => folder.Equals(
                        string.Join(Path.DirectorySeparatorChar, file.Split(Path.DirectorySeparatorChar)[..(file.Split(Path.DirectorySeparatorChar).Length - 1)]))
                    );

                var solution = new ProjectSolutionModel()
                {
                    FileName = file.Split(Path.DirectorySeparatorChar)[file.Split(Path.DirectorySeparatorChar).Length - 1],
                    FilePath = file,
                    ParentFolderName = folder.Split(Path.DirectorySeparatorChar)[folder.Split(Path.DirectorySeparatorChar).Length - 1],
                    ParentFolderPath = folder,
                    reportData = new()
                };

                using var reader = new StreamReader(file);

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
        }
        return null;
    }
}
