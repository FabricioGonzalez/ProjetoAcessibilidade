using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

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
        var files = await StorageFolder.GetFolderFromPathAsync(Path.Combine(path, "Tables"));

        List<FileTemplates> filesList = new List<FileTemplates>();

        filesList.Clear();

        foreach (var item in await files.GetItemsAsync())
        {
            if (item.IsOfType(StorageItemTypes.File))
            {
                if (item.Name.Split(".")[1] == "xml")
                {
                    var file = new FileTemplates();
                    file.Name = item.Name.Split(".")[0];
                    file.Path = item.Path;

                    filesList.Add(file);
                }
            }
        }

        return filesList;
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
        list.Add(folderItem);
    }
    public async Task<ObservableCollection<ExplorerItem>> GetData(string SolutionPath)
    {
        var list = new ObservableCollection<ExplorerItem>();

        //var directory = await StorageFolder.GetFolderFromPathAsync(Path.Combine(Package.Current.InstalledPath, "Specifications"));

        if (Directory.Exists(Path.Combine(SolutionPath, "Itens")))
        {
            var directory = await StorageFolder.GetFolderFromPathAsync(Path.Combine(SolutionPath, "Itens"));

            await GetDataFromPath(directory, list);
        }
        return list;
    }
    public async Task CreateProjectSolutionItem(string projectPath, string ProjectItemName, string refPath)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(projectPath);

        StorageFile refFile = await StorageFile.GetFileFromPathAsync(refPath);

        var file = await folder.CreateFileAsync(Path.Combine(projectPath,ProjectItemName));

        await refFile.CopyAndReplaceAsync(file);
    }
    public async Task DeleteProjectSolutionItem(string projectItemPath)
    {
        var file = await StorageFile.GetFileFromPathAsync(projectItemPath);

        await file.DeleteAsync();
    }
    public async Task CreateProjectSolutionFolder(string projectPath, string ProjectFolder)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(projectPath);

        await folder.CreateFolderAsync(ProjectFolder);
    }
    public async Task DeleteProjectSolutionFolder(string projectFolderPath)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(projectFolderPath);

        await folder.DeleteAsync();
    }
}
