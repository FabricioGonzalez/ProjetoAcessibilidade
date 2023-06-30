using System.Security;

using Common;
using Common.Models;
using Common.Optional;
using Common.Result;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjectItemReader.InternalAppFiles;

public class ExplorerItemRepositoryImpl : IExplorerItemRepository
{
    public Result<ExplorerItem> CreateExplorerItem(
        ExplorerItem item
    ) => Result<ExplorerItem>.Success(new ExplorerItem(Guid.NewGuid()));

    public Result<ExplorerItem> DeleteExplorerItem(
        ExplorerItem item
    ) => Result<ExplorerItem>.Success(new ExplorerItem(Guid.NewGuid()));

    public Result<ExplorerItem> RenameFileItem(
        ExplorerItem item
    ) =>
        throw new NotImplementedException();

    public Result<Empty> MoveFileItem(
        string oldPath,
        string newPath
    )
    {
        return oldPath
             .ToOption()
             .MapValue(item =>
             {
                 try
                 {
                     if (File.Exists(item))
                     {
                         if (!string.IsNullOrWhiteSpace(newPath))
                         {
                             File.Move(item, newPath);
                             return Result<Empty>.Success(Empty.Value);
                         }
                         return Result<Empty>.Failure(new Exception("O destino do arquivo não pode ser nulo"));
                     }
                     return Result<Empty>.Failure(new Exception("O arquivo não existe"));
                 }
                 catch (IOException ex)
                 {
                     return Result<Empty>.Failure(ex);
                 }
             })
             .Reduce(() => Result<Empty>.Failure(new Exception("A Operação não pode ser concluída")));
    }


    public Result<ExplorerItem> DeleteFolderItem(
        ExplorerItem item
    ) =>
        throw new NotImplementedException();

    public Result<IEnumerable<ExplorerItem>> GetAllItems(
        string solutionPath
    )
    {
        return solutionPath
            .ToOption()
            .Map(path =>
            {
                return File.GetAttributes(path: solutionPath) switch
                {
                    FileAttributes.Archive => Path.Combine(
                path1: Directory.GetParent(path: solutionPath)
                .ToOption()
                .Map(item => item.FullName)
                .Reduce(() => ""),
                path2: Constants.AppProjectItemsFolderName),
                    FileAttributes.Directory => Path.Combine(
                path1: solutionPath,
                path2: Constants.AppProjectItemsFolderName),
                    _ => ""
                };
            })
            .Map(storageItem =>
            {
                var items = Enumerable.Empty<ExplorerItem>();

                if (Directory.Exists(path: storageItem))
                {
                    var folderItem = new FolderItem(Guid.NewGuid())
                    {
                        Name = Path.GetDirectoryName(path: storageItem).ToOption().Map(it => it).Reduce(() => "Not Found"),
                        Path = storageItem,
                        Children = new List<ExplorerItem>()
                    };

                    items = items.Append(folderItem);

                    var directory = Directory.GetFileSystemEntries(path: storageItem);

                    GetDataFromPath(
                        folder: directory,
                        list: folderItem.Children);
                }

                return items;
            })
            .MapValue(resultItem =>
        {
            if (resultItem.Count() > 0)
            {
                return Result<IEnumerable<ExplorerItem>>.Success(resultItem);
            }
            return Result<IEnumerable<ExplorerItem>>.Failure(new Exception("Não Há items"));
        })
            .Reduce(() => Result<IEnumerable<ExplorerItem>>.Failure(new Exception("Erro Intero em algum dos Processos")));
    }

    public async Task<Result<IEnumerable<ExplorerItem>>> GetAllItemsAsync(
        string solutionPath
    )
    {
        return solutionPath
            .ToOption()
            .Map(path =>
            {
                return File.GetAttributes(path: solutionPath) switch
                {
                    FileAttributes.Archive => Path.Combine(
                path1: Directory.GetParent(path: solutionPath)
                .ToOption()
                .Map(item => item.FullName)
                .Reduce(() => ""),
                path2: Constants.AppProjectItemsFolderName),
                    FileAttributes.Directory => Path.Combine(
                path1: solutionPath,
                path2: Constants.AppProjectItemsFolderName),
                    _ => ""
                };
            })
            .Map(storageItem =>
            {
                var items = Enumerable.Empty<ExplorerItem>();

                if (Directory.Exists(path: storageItem))
                {
                    var folderItem = new FolderItem(Guid.NewGuid())
                    {
                        Name = Path.GetDirectoryName(path: storageItem).ToOption().Map(it => it).Reduce(() => "Not Found"),
                        Path = storageItem,
                        Children = new List<ExplorerItem>()
                    };

                    items = items.Append(folderItem);

                    var directory = Directory.GetFileSystemEntries(path: storageItem);

                    GetDataFromPath(
                        folder: directory,
                        list: folderItem.Children);
                }

                return items;
            })
            .MapValue(resultItem =>
            {
                if (resultItem.Count() > 0)
                {
                    return Result<IEnumerable<ExplorerItem>>.Success(resultItem);
                }
                return Result<IEnumerable<ExplorerItem>>.Failure(new Exception("Não Há items"));
            })
            .Reduce(() => Result<IEnumerable<ExplorerItem>>.Failure(new Exception("Erro Intero em algum dos Processos")));
    }

    public Result<ExplorerItem> UpdateExplorerItem(
        ExplorerItem item
    ) => Result<ExplorerItem>.Success(new ExplorerItem(Guid.NewGuid()));

    public async Task<Result<ExplorerItem>> UpdateExplorerItemAsync(
        ExplorerItem item
    ) => Result<ExplorerItem>.Success(new ExplorerItem(Guid.NewGuid()));

    public async Task<Result<ExplorerItem>> RenameFileItemAsync(
        ExplorerItem item
    )
    {
        return (await item
            .ToOption()
            .MapValue(explorerItem =>
            {
                return (itemName: explorerItem.Name, itemPath: explorerItem.Path, exists: File.Exists(explorerItem.Path));
            })
            .MapValueAsync(async explorerItem =>
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        if (explorerItem.exists && Directory.GetParent(path: item.Path) is { Exists: true } info)
                        {
                            File.Move(
                          sourceFileName: explorerItem.itemPath,
                          destFileName: Path.Combine(
                              path1: info!.FullName,
                              path2: $"{item.Name}{Constants.AppProjectItemExtension}"));

                            item.Path = explorerItem.itemPath.Replace(
                            oldValue: Path.GetFileName(path: explorerItem.itemPath),
                            newValue: $"{item.Name}{Constants.AppProjectItemExtension}");

                            return Result<ExplorerItem>.Success(item);
                        }

                        return Result<ExplorerItem>.Failure(new Exception("Erro na operação!"));
                    }
                    catch (IOException ex)
                    {
                        return Result<ExplorerItem>.Failure(ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        return Result<ExplorerItem>.Failure(ex);
                    }
                    catch (ArgumentException ex)
                    {
                        return Result<ExplorerItem>.Failure(ex);
                    }
                    catch (NotSupportedException ex)
                    {
                        return Result<ExplorerItem>.Failure(ex);
                    }
                    catch (SecurityException ex)
                    {
                        return Result<ExplorerItem>.Failure(ex);
                    }
                });
            }))
            .Reduce(() => Result<ExplorerItem>.Failure(new Exception("Não foi Possivel finalizar a operação")));
    }


    public async Task<Result<ExplorerItem>> RenameFolderItemAsync(
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
                _ = Directory.CreateDirectory(
                    path: Path.Combine(
                        path1: item.Path,
                        path2: item.Name));
            }

            item.Path = item.Path.Replace(
                oldValue: Path.GetFileName(path: item.Path),
                newValue: item.Name);
        }

        return Result<ExplorerItem>.Success(item);
    }

    public async Task<Result<ExplorerItem>> DeleteFolderItemAsync(
        string itemPath
    )
    {
        if (itemPath is not null)
        {
            if (Directory.Exists(path: itemPath))
            {
                Directory.Delete(path: itemPath, recursive: true);
                return Result<ExplorerItem>.Success(new ExplorerItem(Guid.NewGuid()));
            }
        }
        return Result<ExplorerItem>.Failure(new Exception("Diretório não existe"));
    }

    public async Task<Result<ExplorerItem>> DeleteFileItemAsync(
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

                return Result<ExplorerItem>.Success(new ExplorerItem(Guid.NewGuid()));
            }
        }

        return Result<ExplorerItem>.Failure(new Exception("Erro ao deletar arquivo"));
    }

    public Result<ExplorerItem> RenameFolderItem(
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

        return Result<ExplorerItem>.Success(new ExplorerItem(Guid.NewGuid()));
    }

    public Result<ExplorerItem> DeleteFileItem(
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

        return Result<ExplorerItem>.Success(new ExplorerItem(Guid.NewGuid()));
    }

    public Result<Empty> RenameFolderItem(
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
                _ = Directory.CreateDirectory(path: itemPath);
            }
        }

        return Result<Empty>.Success(new Empty());
    }

    public async Task<Result<ExplorerItem>> CreateExplorerItemAsync(
        ExplorerItem item
    ) => Result<ExplorerItem>.Success(new ExplorerItem(Guid.NewGuid()));

    public async Task<Result<ExplorerItem>> DeleteExplorerItemAsync(
        ExplorerItem item
    ) => Result<ExplorerItem>.Success(new ExplorerItem(Guid.NewGuid()));

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