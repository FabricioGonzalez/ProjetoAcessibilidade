using AppUsecases.Contracts.Repositories;
using AppUsecases.Project.Entities.FileTemplate;

using Common;

using Project.Application.Project.Contracts;

namespace ProjectItemReader.InternalAppFiles;
public class ReadProjectFilesRepository : IReadContract<List<ExplorerItem>>
{
    public async Task<List<ExplorerItem>> ReadAsync(string path)
    {
        var list = new List<ExplorerItem>();

        var fileAttributes = File.GetAttributes(path);

        var projectItemsRootPath = "";

        if (fileAttributes.HasFlag(FileAttributes.Archive))
        {
            projectItemsRootPath = Path.Combine(Directory.GetParent(path).FullName, Constants.AppProjectItemsFolderName);
        }
        if (fileAttributes.HasFlag(FileAttributes.Directory))
        {
            Path.Combine(path, Constants.AppProjectItemsFolderName);
        }

        if (Directory.Exists(projectItemsRootPath))
        {
            var folderItem = new FolderItem()
            {
                Name = Path.GetFileNameWithoutExtension(projectItemsRootPath),
                Path = projectItemsRootPath
            };

            folderItem.Children = new List<ExplorerItem>();

            list.Add(folderItem);

            var directory = Directory.GetFileSystemEntries(projectItemsRootPath);

            await GetDataFromPath(directory, folderItem.Children);
        }
        return list;
    }
    private async Task GetDataFromPath(string[] folder, IList<ExplorerItem> list)
    {
        foreach (var item in folder)
        {
            var fileAttributes = File.GetAttributes(item);

            if (File.GetAttributes(item).HasFlag(FileAttributes.Archive))
            {
                var i = new FileItem()
                {
                    Name = Path.GetFileNameWithoutExtension(item),
                    Path = item,
                };
                list.Add(i);
            }

            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(item);

                var folderItem = new FolderItem
                {
                    Name = Path.GetDirectoryName(item),
                    Path = item,
                    Children = new List<ExplorerItem>()
                };

                foreach (var fileItem in itens)
                {
                    if (fileAttributes.HasFlag(FileAttributes.Directory))
                    {
                        var entries = Directory.GetFileSystemEntries(fileItem);

                        await GetDataFromPath(entries, folderItem.Children);
                    }
                    if (File.GetAttributes(item).HasFlag(FileAttributes.Archive))
                    {
                        var i = new FileItem()
                        {
                            Name = Path.GetFileNameWithoutExtension(item),
                            Path = item,
                        };
                        folderItem.Children.Add(i);
                    }
                }

                list.Add(folderItem);
            }
        }
    }
}
public class ExplorerItemRepositoryImpl : IExplorerItemRepository
{
    public Resource<App.Core.Entities.Solution.Explorer.ExplorerItem> CreateExplorerItem(App.Core.Entities.Solution.Explorer.ExplorerItem item)
    {
        return new Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>.Success(new());

    }
    public async Task<Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>> CreateExplorerItemAsync(App.Core.Entities.Solution.Explorer.ExplorerItem item)
    {
        return new Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>.Success(new());

    }
   
    public Resource<App.Core.Entities.Solution.Explorer.ExplorerItem> DeleteExplorerItem(App.Core.Entities.Solution.Explorer.ExplorerItem item)
    {
        return new Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>.Success(new());

    }
    public async Task<Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>> DeleteExplorerItemAsync(App.Core.Entities.Solution.Explorer.ExplorerItem item)
    {
        return new Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>.Success(new());

    }
   
    public Resource<List<App.Core.Entities.Solution.Explorer.ExplorerItem>> GetAllItems(string solutionPath)
    {
        var list = new List<App.Core.Entities.Solution.Explorer.ExplorerItem>();

        var fileAttributes = File.GetAttributes(solutionPath);

        var projectItemsRootPath = "";

        if (fileAttributes.HasFlag(FileAttributes.Archive))
        {
            projectItemsRootPath = Path.Combine(Directory.GetParent(solutionPath).FullName, Constants.AppProjectItemsFolderName);
        }
        if (fileAttributes.HasFlag(FileAttributes.Directory))
        {
            Path.Combine(solutionPath, Constants.AppProjectItemsFolderName);
        }

        if (Directory.Exists(projectItemsRootPath))
        {
            var folderItem = new App.Core.Entities.Solution.Explorer.FolderItem()
            {
                Name = Path.GetFileNameWithoutExtension(projectItemsRootPath),
                Path = projectItemsRootPath
            };

            folderItem.Children = new List<App.Core.Entities.Solution.Explorer.ExplorerItem>();

            list.Add(folderItem);

            var directory = Directory.GetFileSystemEntries(projectItemsRootPath);

            GetDataFromPath(directory, folderItem.Children);
        }
        if (list.Count > 0)
        {
            return new Resource<List<App.Core.Entities.Solution.Explorer.ExplorerItem>>.Success(list);
        }
        return new Resource<List<App.Core.Entities.Solution.Explorer.ExplorerItem>>.Error("Erro ao Ler itens", null);
    }
    public async Task<Resource<List<App.Core.Entities.Solution.Explorer.ExplorerItem>>> GetAllItemsAsync(string solutionPath)
    {
        var list = new List<App.Core.Entities.Solution.Explorer.ExplorerItem>();

        var fileAttributes = File.GetAttributes(solutionPath);

        var projectItemsRootPath = "";

        if (fileAttributes.HasFlag(FileAttributes.Archive))
        {
            projectItemsRootPath = Path.Combine(Directory.GetParent(solutionPath).FullName, Constants.AppProjectItemsFolderName);
        }
        if (fileAttributes.HasFlag(FileAttributes.Directory))
        {
            Path.Combine(solutionPath, Constants.AppProjectItemsFolderName);
        }

        if (Directory.Exists(projectItemsRootPath))
        {
            var folderItem = new App.Core.Entities.Solution.Explorer.FolderItem()
            {
                Name = Path.GetFileNameWithoutExtension(projectItemsRootPath),
                Path = projectItemsRootPath
            };

            list.Add(folderItem);

            var directory = Directory.GetFileSystemEntries(projectItemsRootPath);

            await GetDataFromPathAsync(directory, folderItem.Children);
        }
        if (list.Count > 0)
        {
            return new Resource<List<App.Core.Entities.Solution.Explorer.ExplorerItem>>.Success(list);
        }
        return new Resource<List<App.Core.Entities.Solution.Explorer.ExplorerItem>>.Error("Erro ao Ler itens", null);
    }

    public Resource<App.Core.Entities.Solution.Explorer.ExplorerItem> UpdateExplorerItem(App.Core.Entities.Solution.Explorer.ExplorerItem item)
    {
        return new Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>.Success(new());
    }
    public async Task<Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>> UpdateExplorerItemAsync(App.Core.Entities.Solution.Explorer.ExplorerItem item)
    {
        return new Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>.Success(new());

    }

    private async Task GetDataFromPathAsync(string[] folder, IList<App.Core.Entities.Solution.Explorer.ExplorerItem> list)
    {
        foreach (var item in folder)
        {
            var fileAttributes = File.GetAttributes(item);

            if (File.GetAttributes(item).HasFlag(FileAttributes.Archive))
            {
                var i = new App.Core.Entities.Solution.Explorer.FileItem()
                {
                    Name = Path.GetFileNameWithoutExtension(item),
                    Path = item,
                };
                list.Add(i);
            }

            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(item);

                var folderItem = new App.Core.Entities.Solution.Explorer.FolderItem
                {
                    Name = Path.GetDirectoryName(item),
                    Path = item,
                };

                foreach (var fileItem in itens)
                {
                    if (fileAttributes.HasFlag(FileAttributes.Directory))
                    {
                        var entries = Directory.GetFileSystemEntries(fileItem);

                        await GetDataFromPathAsync(entries, folderItem.Children);
                    }
                    if (File.GetAttributes(item).HasFlag(FileAttributes.Archive))
                    {
                        var i = new App.Core.Entities.Solution.Explorer.FileItem()
                        {
                            Name = Path.GetFileNameWithoutExtension(item),
                            Path = item,
                        };
                        folderItem.Children.Add(i);
                    }
                }

                list.Add(folderItem);
            }
        }
    }

    private void GetDataFromPath(string[] folder, IList<App.Core.Entities.Solution.Explorer.ExplorerItem> list)
    {
        foreach (var item in folder)
        {
            var fileAttributes = File.GetAttributes(item);

            if (File.GetAttributes(item).HasFlag(FileAttributes.Archive))
            {
                var i = new App.Core.Entities.Solution.Explorer.FileItem()
                {
                    Name = Path.GetFileNameWithoutExtension(item),
                    Path = item,
                };
                list.Add(i);
            }

            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(item);

                var folderItem = new App.Core.Entities.Solution.Explorer.FolderItem
                {
                    Name = Path.GetDirectoryName(item),
                    Path = item,
                };

                foreach (var fileItem in itens)
                {
                    if (fileAttributes.HasFlag(FileAttributes.Directory))
                    {
                        var entries = Directory.GetFileSystemEntries(fileItem);

                        GetDataFromPath(entries, folderItem.Children);
                    }
                    if (File.GetAttributes(item).HasFlag(FileAttributes.Archive))
                    {
                        var i = new App.Core.Entities.Solution.Explorer.FileItem()
                        {
                            Name = Path.GetFileNameWithoutExtension(item),
                            Path = item,
                        };
                        folderItem.Children.Add(i);
                    }
                }

                list.Add(folderItem);
            }
        }
    }

    public async Task<Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>> RenameExplorerItemAsync(App.Core.Entities.Solution.Explorer.ExplorerItem item)
    {
        return new Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>.Success(new());
    }
    public Resource<App.Core.Entities.Solution.Explorer.ExplorerItem> RenameExplorerItem(App.Core.Entities.Solution.Explorer.ExplorerItem item)
    {
        return new Resource<App.Core.Entities.Solution.Explorer.ExplorerItem>.Success(new());
    }
}
