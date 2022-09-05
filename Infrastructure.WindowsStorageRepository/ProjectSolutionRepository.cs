using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Projeto.Core.Models;

using SystemApplication.Services.Contracts;
using SystemApplication.Services.UIOutputs;

using Windows.Storage;

namespace Infrastructure.WindowsStorageRepository;

public class ProjectSolutionRepository : IProjectSolutionRepository
{
    readonly string path;
    public ProjectSolutionRepository(string path)
    {
        this.path = path;
    }
    public async Task<List<FileTemplates>> getProjectLocalPath()
    {
        try
        {
            var files = await StorageFolder.GetFolderFromPathAsync(Path.Combine(path, "Tables"));

            List<FileTemplates> filesList = new List<FileTemplates>();

            filesList.Clear();

            foreach (var item in await files.GetItemsAsync())
            {
                if (item.IsOfType(StorageItemTypes.File))
                {
                    var i = item.Name.Split(".")[1];
                    if (i.Equals("xml") || i.Equals("prja") || i.Equals("prjd"))
                    {
                        var file = new FileTemplates();
                        file.Name = item.Name.Split(".")[0];
                        file.Path = item.Path;

                        filesList.Add(file);
                    }
                }
            }
            files = null;

            return filesList;
        }
        catch (Exception)
        {
            throw;
        }
    }
    private async Task GetDataFromPath(StorageFolder folder, IList<ExplorerItem> list)
    {
        var itens = await folder.GetItemsAsync();

        var folderItem = new ExplorerItem
        {
            Name = folder.Name,
            Path = folder.Path,
            Type = ExplorerItem.ExplorerItemType.Folder,
            Children = new()
        };

        foreach (var item in itens)
        {
            if (item.IsOfType(StorageItemTypes.Folder))
            {
                var newfolder = await StorageFolder.GetFolderFromPathAsync(item.Path);

                await GetDataFromPath(newfolder, folderItem.Children);
            }
            if (item.IsOfType(StorageItemTypes.File))
            {
                var i = new ExplorerItem()
                {
                    Name = item.Name.Split(".")[0],
                    Path = item.Path,
                    Type = ExplorerItem.ExplorerItemType.File
                };
                folderItem.Children.Add(i);
            }
        }
        itens = null;
        list.Add(folderItem);
    }
    public async Task<ObservableCollection<ExplorerItem>> GetData(string SolutionPath)
    {
        var list = new ObservableCollection<ExplorerItem>();

        if (Directory.Exists(Path.Combine(SolutionPath, "Itens")))
        {
            var directory = await StorageFolder.GetFolderFromPathAsync(Path.Combine(SolutionPath, "Itens"));

            await GetDataFromPath(directory, list);
            directory = null;
        }
        return list;
    }
    public async Task CreateProjectSolutionItem(string projectPath, string ProjectItemName, string refPath)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(projectPath);

        StorageFile refFile = await StorageFile.GetFileFromPathAsync(refPath);

        var file = await folder.CreateFileAsync(ProjectItemName, CreationCollisionOption.GenerateUniqueName);

        await refFile.CopyAndReplaceAsync(file);

        file = null;
        folder = null;
        refFile = null;
    }
    public async Task DeleteProjectSolutionItem(string projectItemPath)
    {
        var file = await StorageFile.GetFileFromPathAsync(projectItemPath);

        await file.DeleteAsync(StorageDeleteOption.Default);
        file = null;
    }
    public async Task CreateProjectSolutionFolder(string projectPath, string ProjectFolder)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(projectPath);

        await folder.CreateFolderAsync(ProjectFolder, CreationCollisionOption.GenerateUniqueName);
        folder = null;
    }
    public async Task DeleteProjectSolutionFolder(string projectFolderPath)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(projectFolderPath);

        await folder.DeleteAsync(StorageDeleteOption.Default);
        folder = null;
    }
    public async Task RenameProjectFolder(string projectPath, string ProjectItemName)
    {
        var item = await StorageFolder.GetFolderFromPathAsync(projectPath);
        await item.RenameAsync(ProjectItemName, NameCollisionOption.GenerateUniqueName);

        item = null;
    }
    public async Task RenameProjectItem(string projectPath, string ProjectItemName)
    {
        var item = await StorageFile.GetFileFromPathAsync(projectPath);
        await item.RenameAsync(ProjectItemName, NameCollisionOption.GenerateUniqueName);

        item = null;
    }
    public async Task<ProjectSolutionModel>? GetProjectSolutionData(string projectPath)
    {
        var file = await StorageFile.GetFileFromPathAsync(projectPath);
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
    public async Task<ProjectSolutionModel>? SaveProjectSolutionData(string projectPath)
    {
        var file = await StorageFile.GetFileFromPathAsync(projectPath);
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

            using (var writer = await file.OpenStreamForWriteAsync())
            {
                await writer.WriteAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new ReportDataModel())));

                using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
                {
                    await folder.CreateFolderAsync("Itens");

                    if (await reader.ReadToEndAsync() is string data && data.Length > 0)
                    {
                        var resultData = JsonSerializer.Deserialize<ReportDataModel>(data) ?? null;

                        if (resultData is not null)
                            solution.reportData = resultData;
                    }

                    return solution;
                }
            }
        }
        return null;
    }
}
