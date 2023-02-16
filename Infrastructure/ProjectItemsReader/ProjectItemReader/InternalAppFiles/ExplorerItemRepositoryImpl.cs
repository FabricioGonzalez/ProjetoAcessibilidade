using Common;

using Core.Entities.Solution.Explorer;

using Project.Application.Project.Contracts;

namespace ProjectItemReader.publicAppFiles;

public class ExplorerItemRepositoryImpl : IExplorerItemRepository
{
    public Resource<ExplorerItem> CreateExplorerItem(ExplorerItem item)
    {
        return new Resource<ExplorerItem>.Success(new());

    }
    public async Task<Resource<ExplorerItem>> CreateExplorerItemAsync(ExplorerItem item)
    {
        return new Resource<ExplorerItem>.Success(new());

    }

    public Resource<ExplorerItem> DeleteExplorerItem(ExplorerItem item)
    {
        return new Resource<ExplorerItem>.Success(new());

    }
    public async Task<Resource<ExplorerItem>> DeleteExplorerItemAsync(ExplorerItem item)
    {
        return new Resource<ExplorerItem>.Success(new());

    }

    public Resource<List<ExplorerItem>> GetAllItems(string solutionPath)
    {
        var list = new List<ExplorerItem>();

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
            var folderItem = new FolderItem
            {
                Name = Path.GetFileNameWithoutExtension(projectItemsRootPath),
                Path = projectItemsRootPath,
                Children = new List<ExplorerItem>()
            };

            list.Add(folderItem);

            var directory = Directory.GetFileSystemEntries(projectItemsRootPath);

            GetDataFromPath(directory, folderItem.Children);
        }
        if (list.Count > 0)
        {
            return new Resource<List<ExplorerItem>>.Success(list);
        }
        return new Resource<List<ExplorerItem>>.Error("Erro ao Ler itens", null);
    }
    public async Task<Resource<List<ExplorerItem>>> GetAllItemsAsync(string solutionPath)
    {
        var list = new List<ExplorerItem>();

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
            var folderItem = new FolderItem()
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
            return new Resource<List<ExplorerItem>>.Success(list);
        }
        return new Resource<List<ExplorerItem>>.Error("Erro ao Ler itens", null);
    }

    public Resource<ExplorerItem> UpdateExplorerItem(ExplorerItem item)
    {
        return new Resource<ExplorerItem>.Success(new());
    }
    public async Task<Resource<ExplorerItem>> UpdateExplorerItemAsync(ExplorerItem item)
    {
        return new Resource<ExplorerItem>.Success(new());

    }

    private async Task GetDataFromPathAsync(string[] folder, IList<ExplorerItem> list)
    {
        foreach (var item in folder)
        {
            if (File.GetAttributes(item).HasFlag(FileAttributes.Archive))
            {
                var i = new FileItem()
                {
                    Name = Path.GetFileNameWithoutExtension(item),
                    Path = item,
                };
                list.Add(i);
            }

            if (File.GetAttributes(item).HasFlag(FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(item);

                var folderItem = new FolderItem
                {
                    Name = Path.GetFileNameWithoutExtension(item),
                    Path = item,
                };

                foreach (var fileItem in itens)
                {
                    if (File.GetAttributes(fileItem).HasFlag(FileAttributes.Directory))
                    {
                        var entries = Directory.GetFileSystemEntries(fileItem);

                        await GetDataFromPathAsync(entries, folderItem.Children);
                    }
                    if (File.GetAttributes(fileItem).HasFlag(FileAttributes.Archive))
                    {
                        var i = new FileItem()
                        {
                            Name = Path.GetFileNameWithoutExtension(fileItem),
                            Path = fileItem,
                        };
                        folderItem.Children.Add(i);
                    }
                }

                list.Add(folderItem);
            }
        }
    }

    private void GetDataFromPath(string[] folder, IList<ExplorerItem> list)
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

    public async Task<Resource<ExplorerItem>> RenameFileItemAsync(ExplorerItem item)
    {
        var path = Directory.GetParent(item.Path).FullName;
        if (item is not null)
        {
            if (File.Exists(item.Path))
            {
                File.Move(item.Path, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(item.ReferencedItem, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            item.Path = item.Path.Replace(Path.GetFileName(item.Path), $"{item.Name}{Constants.AppProjectItemExtension}");
        }
        return new Resource<ExplorerItem>.Success(new());
    }
    public Resource<ExplorerItem> RenameFileItem(ExplorerItem item)
    {
        return new Resource<ExplorerItem>.Success(new());
    }

    public async Task<Resource<ExplorerItem>> RenameFolderItemAsync(ExplorerItem item)
    {
        if (item is not null)
        {
            if (Directory.Exists(item.Path))
            {
                Directory.Move(item.Path, Path.Combine(Directory.GetParent(item.Path).FullName, item.Name));
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(item.Path, item.Name));
            }
            item.Path = item.Path.Replace(Path.GetFileName(item.Path), item.Name);
        }
        return new Resource<ExplorerItem>.Success(item);
    }
    public async Task<Resource<ExplorerItem>> DeleteFolderItemAsync(ExplorerItem item)
    {
        var path = Directory.GetParent(item.Path).FullName;
        if (item is not null)
        {
            if (File.Exists(item.Path))
            {
                File.Move(item.Path, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(item.ReferencedItem, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            item.Path = item.Path.Replace(Path.GetFileName(item.Path), $"{item.Name}{Constants.AppProjectItemExtension}");
        }
        return new Resource<ExplorerItem>.Success(new());
    }
    public async Task<Resource<ExplorerItem>> DeleteFileItemAsync(ExplorerItem item)
    {
        var path = Directory.GetParent(item.Path).FullName;
        if (item is not null)
        {
            if (File.Exists(item.Path))
            {
                File.Move(item.Path, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(item.ReferencedItem, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            item.Path = item.Path.Replace(Path.GetFileName(item.Path), $"{item.Name}{Constants.AppProjectItemExtension}");
        }
        return new Resource<ExplorerItem>.Success(new());
    }
    public Resource<ExplorerItem> RenameFolderItem(ExplorerItem item)
    {
        var path = Directory.GetParent(item.Path).FullName;
        if (item is not null)
        {
            if (File.Exists(item.Path))
            {
                File.Move(item.Path, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(item.ReferencedItem, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            item.Path = item.Path.Replace(Path.GetFileName(item.Path), $"{item.Name}{Constants.AppProjectItemExtension}");
        }
        return new Resource<ExplorerItem>.Success(new());
    }
    public Resource<ExplorerItem> DeleteFolderItem(ExplorerItem item)
    {
        var path = Directory.GetParent(item.Path).FullName;
        if (item is not null)
        {
            if (File.Exists(item.Path))
            {
                File.Move(item.Path, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(item.ReferencedItem, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            item.Path = item.Path.Replace(Path.GetFileName(item.Path), $"{item.Name}{Constants.AppProjectItemExtension}");
        }
        return new Resource<ExplorerItem>.Success(new());
    }
    public Resource<ExplorerItem> DeleteFileItem(ExplorerItem item)
    {
        var path = Directory.GetParent(item.Path).FullName;
        if (item is not null)
        {
            if (File.Exists(item.Path))
            {
                File.Move(item.Path, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(item.ReferencedItem, Path.Combine(path, $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            item.Path = item.Path.Replace(Path.GetFileName(item.Path), $"{item.Name}{Constants.AppProjectItemExtension}");
        }
        return new Resource<ExplorerItem>.Success(new());
    }
}
