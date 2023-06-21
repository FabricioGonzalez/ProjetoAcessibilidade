using Common;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjectItemReader.InternalAppFiles;

public class ExplorerItemRepositoryImpl : IExplorerItemRepository
{
    public Resource<ExplorerItem> CreateExplorerItem(
        ExplorerItem item
    ) => new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));

    public Resource<ExplorerItem> DeleteExplorerItem(
        ExplorerItem item
    ) => new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));

    public Resource<ExplorerItem> RenameFileItem(
        ExplorerItem item
    ) =>
        throw new NotImplementedException();

    public Resource<ExplorerItem> DeleteFolderItem(
        ExplorerItem item
    ) =>
        throw new NotImplementedException();

    public Resource<List<ExplorerItem>> GetAllItems(
        string solutionPath
    )
    {
        var list = new List<ExplorerItem>();

        var fileAttributes = File.GetAttributes(path: solutionPath);

        var projectItemsRootPath = "";

        if (fileAttributes.HasFlag(flag: FileAttributes.Archive))
        {
            projectItemsRootPath = Path.Combine(
                path1: Directory.GetParent(path: solutionPath)
                    .FullName,
                path2: Constants.AppProjectItemsFolderName);
        }

        if (fileAttributes.HasFlag(flag: FileAttributes.Directory))
        {
            Path.Combine(
                path1: solutionPath,
                path2: Constants.AppProjectItemsFolderName);
        }

        if (Directory.Exists(path: projectItemsRootPath))
        {
            var folderItem = new FolderItem(Guid.NewGuid())
            {
                Name = Path.GetFileNameWithoutExtension(path: projectItemsRootPath),
                Path = projectItemsRootPath,
                Children = new List<ExplorerItem>()
            };

            list.Add(item: folderItem);

            var directory = Directory.GetFileSystemEntries(path: projectItemsRootPath);

            GetDataFromPath(
                folder: directory,
                list: folderItem.Children);
        }

        if (list.Count > 0)
        {
            return new Resource<List<ExplorerItem>>.Success(Data: list);
        }

        return new Resource<List<ExplorerItem>>.Error(
            Message: "Erro ao Ler itens",
            Data: null);
    }

    public async Task<Resource<List<ExplorerItem>>> GetAllItemsAsync(
        string solutionPath
    )
    {
        var list = new List<ExplorerItem>();

        var fileAttributes = File.GetAttributes(path: solutionPath);

        var projectItemsRootPath = "";

        if (fileAttributes.HasFlag(flag: FileAttributes.Archive))
        {
            projectItemsRootPath = Path.Combine(
                path1: Directory.GetParent(path: solutionPath)
                    .FullName,
                path2: Constants.AppProjectItemsFolderName);
        }

        if (fileAttributes.HasFlag(flag: FileAttributes.Directory))
        {
            Path.Combine(
                path1: solutionPath,
                path2: Constants.AppProjectItemsFolderName);
        }

        if (Directory.Exists(path: projectItemsRootPath))
        {
            var folderItem = new FolderItem(Guid.NewGuid())
            {
                Name = Path.GetFileNameWithoutExtension(path: projectItemsRootPath),
                Path = projectItemsRootPath
            };

            list.Add(item: folderItem);

            var directory = Directory.GetFileSystemEntries(path: projectItemsRootPath);

            await GetDataFromPathAsync(
                folder: directory,
                list: folderItem.Children);
        }

        if (list.Count > 0)
        {
            return new Resource<List<ExplorerItem>>.Success(Data: list);
        }

        return new Resource<List<ExplorerItem>>.Error(
            Message: "Erro ao Ler itens",
            Data: null);
    }

    public Resource<ExplorerItem> UpdateExplorerItem(
        ExplorerItem item
    ) => new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));

    public async Task<Resource<ExplorerItem>> UpdateExplorerItemAsync(
        ExplorerItem item
    ) => new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));

    public async Task<Resource<ExplorerItem>> RenameFileItemAsync(
        ExplorerItem item
    )
    {
        var path = Directory.GetParent(path: item.Path)
            .FullName;
        if (item is not null)
        {
            if (File.Exists(path: item.Path))
            {
                File.Move(
                    sourceFileName: item.Path,
                    destFileName: Path.Combine(
                        path1: path,
                        path2: $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(
                    sourceFileName: item.ReferencedItem,
                    destFileName: Path.Combine(
                        path1: path,
                        path2: $"{item.Name}{Constants.AppProjectItemExtension}"));
            }

            item.Path = item.Path.Replace(
                oldValue: Path.GetFileName(path: item.Path),
                newValue: $"{item.Name}{Constants.AppProjectItemExtension}");
        }

        return new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));
    }


    public async Task<Resource<ExplorerItem>> RenameFolderItemAsync(
        ExplorerItem item
    )
    {
        if (item is not null)
        {
            if (Directory.Exists(path: item.Path))
            {
                Directory.Move(
                    sourceDirName: item.Path,
                    destDirName: Path.Combine(
                        path1: Directory.GetParent(path: item.Path)
                            .FullName,
                        path2: item.Name));
            }
            else
            {
                Directory.CreateDirectory(
                    path: Path.Combine(
                        path1: item.Path,
                        path2: item.Name));
            }

            item.Path = item.Path.Replace(
                oldValue: Path.GetFileName(path: item.Path),
                newValue: item.Name);
        }

        return new Resource<ExplorerItem>.Success(Data: item);
    }

    public async Task<Resource<ExplorerItem>> DeleteFolderItemAsync(
        string itemPath
    )
    {
        if (itemPath is not null)
        {
            if (Directory.Exists(path: itemPath))
            {
                Directory.Delete(path: itemPath, recursive: true);
                return new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));
            }
        }
        return new Resource<ExplorerItem>.Error(Message: "Diretório não existe", Data: new ExplorerItem(Guid.NewGuid()));
    }

    public async Task<Resource<ExplorerItem>> DeleteFileItemAsync(
        string itemPath
    )
    {
        if (itemPath is not null)
        {
            if (File.Exists(path: itemPath))
            {
                await Task.Run(() =>
                {
                    File.Delete(path: itemPath);
                });

                return new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));
            }
        }

        return new Resource<ExplorerItem>.Error(Message: "Erro ao deletar arquivo", Data: new ExplorerItem(Guid.NewGuid()));
    }

    public Resource<ExplorerItem> RenameFolderItem(
        ExplorerItem item
    )
    {
        var path = Directory.GetParent(path: item.Path)
            .FullName;
        if (item is not null)
        {
            if (File.Exists(path: item.Path))
            {
                File.Move(
                    sourceFileName: item.Path,
                    destFileName: Path.Combine(
                        path1: path,
                        path2: $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(
                    sourceFileName: item.ReferencedItem,
                    destFileName: Path.Combine(
                        path1: path,
                        path2: $"{item.Name}{Constants.AppProjectItemExtension}"));
            }

            item.Path = item.Path.Replace(
                oldValue: Path.GetFileName(path: item.Path),
                newValue: $"{item.Name}{Constants.AppProjectItemExtension}");
        }

        return new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));
    }

    public Resource<ExplorerItem> DeleteFileItem(
        ExplorerItem item
    )
    {
        var path = Directory.GetParent(path: item.Path)
            .FullName;
        if (item is not null)
        {
            if (File.Exists(path: item.Path))
            {
                File.Move(
                    sourceFileName: item.Path,
                    destFileName: Path.Combine(
                        path1: path,
                        path2: $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(
                    sourceFileName: item.ReferencedItem,
                    destFileName: Path.Combine(
                        path1: path,
                        path2: $"{item.Name}{Constants.AppProjectItemExtension}"));
            }

            item.Path = item.Path.Replace(
                oldValue: Path.GetFileName(path: item.Path),
                newValue: $"{item.Name}{Constants.AppProjectItemExtension}");
        }

        return new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));
    }

    public Resource<Empty> RenameFolderItem(
        string itemName,
        string itemPath
    )
    {
        if (!string.IsNullOrWhiteSpace(value: itemName) &&
            !string.IsNullOrWhiteSpace(value: itemPath))
        {
            if (Directory.Exists(path: itemPath))
            {
                if (Path.Combine(
                        path1: Directory.GetParent(path: itemPath)
                            .FullName,
                        path2: itemName) !=
                    itemPath)
                {
                    Directory.Move(
                        sourceDirName: itemPath,
                        destDirName: Path.Combine(
                            path1: Directory.GetParent(path: itemPath)
                                .FullName,
                            path2: itemName));
                }
            }
            else
            {
                Directory.CreateDirectory(path: itemPath);
            }
        }

        return new Resource<Empty>.Success(Data: new Empty());
    }

    public async Task<Resource<ExplorerItem>> CreateExplorerItemAsync(
        ExplorerItem item
    ) => new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));

    public async Task<Resource<ExplorerItem>> DeleteExplorerItemAsync(
        ExplorerItem item
    ) => new Resource<ExplorerItem>.Success(Data: new ExplorerItem(Guid.NewGuid()));

    private async Task GetDataFromPathAsync(
        string[] folder,
        IList<ExplorerItem> list
    )
    {
        foreach (var item in folder)
        {
            if (File.GetAttributes(path: item)
                .HasFlag(flag: FileAttributes.Archive))
            {
                var i = new FileItem(Guid.NewGuid())
                {
                    Name = Path.GetFileNameWithoutExtension(path: item),
                    Path = item
                };
                list.Add(item: i);
            }

            if (File.GetAttributes(path: item)
                .HasFlag(flag: FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(path: item);

                var folderItem = new FolderItem(Guid.NewGuid())
                {
                    Name = Path.GetFileNameWithoutExtension(path: item),
                    Path = item
                };

                foreach (var fileItem in itens)
                {
                    if (File.GetAttributes(path: fileItem)
                        .HasFlag(flag: FileAttributes.Directory))
                    {
                        var entries = Directory.GetFileSystemEntries(path: fileItem);

                        await GetDataFromPathAsync(
                            folder: entries,
                            list: folderItem.Children);
                    }

                    if (File.GetAttributes(path: fileItem)
                        .HasFlag(flag: FileAttributes.Archive))
                    {
                        var i = new FileItem(Guid.NewGuid())
                        {
                            Name = Path.GetFileNameWithoutExtension(path: fileItem),
                            Path = fileItem
                        };
                        folderItem.Children.Add(item: i);
                    }
                }

                list.Add(item: folderItem);
            }
        }
    }

    private void GetDataFromPath(
        string[] folder,
        IList<ExplorerItem> list
    )
    {
        foreach (var item in folder)
        {
            var fileAttributes = File.GetAttributes(path: item);

            if (File.GetAttributes(path: item)
                .HasFlag(flag: FileAttributes.Archive))
            {
                var i = new FileItem(Guid.NewGuid())
                {
                    Name = Path.GetFileNameWithoutExtension(path: item),
                    Path = item
                };
                list.Add(item: i);
            }

            if (fileAttributes.HasFlag(flag: FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(path: item);

                var folderItem = new FolderItem(Guid.NewGuid())
                {
                    Name = Path.GetDirectoryName(path: item),
                    Path = item
                };

                foreach (var fileItem in itens)
                {
                    if (fileAttributes.HasFlag(flag: FileAttributes.Directory))
                    {
                        var entries = Directory.GetFileSystemEntries(path: fileItem);

                        GetDataFromPath(
                            folder: entries,
                            list: folderItem.Children);
                    }

                    if (File.GetAttributes(path: item)
                        .HasFlag(flag: FileAttributes.Archive))
                    {
                        var i = new FileItem(Guid.NewGuid())
                        {
                            Name = Path.GetFileNameWithoutExtension(path: item),
                            Path = item
                        };
                        folderItem.Children.Add(item: i);
                    }
                }

                list.Add(item: folderItem);
            }
        }
    }
}