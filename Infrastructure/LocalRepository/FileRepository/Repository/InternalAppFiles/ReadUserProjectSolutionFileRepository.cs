﻿using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Entities;

using Windows.Storage;

namespace LocalRepository.FileRepository.Repository.InternalAppFiles;
internal class ReadUserProjectSolutionFileRepository : IReadContract<ProjectSolutionModel>
{
    public async Task<ProjectSolutionModel> ReadAsync(string path)
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
        return null;
    }
}