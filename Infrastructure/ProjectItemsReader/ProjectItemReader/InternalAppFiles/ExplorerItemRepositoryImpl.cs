using System.Security;
using Common;
using Common.Models;
using Common.Optional;
using LanguageExt.Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjectItemReader.InternalAppFiles;

public class ExplorerItemRepositoryImpl : IExplorerItemRepository
{
    public Result<ExplorerItem> CreateExplorerItem(
        ExplorerItem item
    ) => new(new ExplorerItem(Guid.NewGuid()));

    public Result<ExplorerItem> DeleteExplorerItem(
        ExplorerItem item
    ) => new(new ExplorerItem(Guid.NewGuid()));

    public Result<ExplorerItem> RenameFileItem(
        ExplorerItem item
    ) =>
        throw new NotImplementedException();


    public Result<ExplorerItem> DeleteFolderItem(
        ExplorerItem item
    ) =>
        throw new NotImplementedException();

    public Result<IEnumerable<ExplorerItem>> GetAllItems(
        string solutionPath
    ) =>
        OptionalResult<string>.Optional(solutionPath)
            .Map(path =>
            {
                return File.GetAttributes(solutionPath) switch
                {
                    FileAttributes.Archive => Path.Combine(
                        Directory.GetParent(solutionPath)
                            .ToOption()
                            .Map(item => item.FullName)
                            .Reduce(() => ""),
                        Constants.AppProjectItemsFolderName)
                    , FileAttributes.Directory => Path.Combine(
                        solutionPath,
                        Constants.AppProjectItemsFolderName)
                    , _ => ""
                };
            })
            .Map(storageItem =>
            {
                var items = Enumerable.Empty<ExplorerItem>();

                if (Directory.Exists(storageItem))
                {
                    var folderItem = new FolderItem(Guid.NewGuid())
                    {
                        Name = Path.GetDirectoryName(storageItem)
                        , Path = storageItem, Children = new List<ExplorerItem>()
                    };

                    items = items.Append(folderItem);

                    var directory = Directory.GetFileSystemEntries(storageItem);

                    GetDataFromPath(
                        directory,
                        folderItem.Children);
                }

                return items;
            })
            .Match(resultItem =>
                {
                    return resultItem.Any()
                        ? new Result<IEnumerable<ExplorerItem>>(resultItem)
                        : new Result<IEnumerable<ExplorerItem>>(new Exception("Não Há items"));
                },
                () => new Result<IEnumerable<ExplorerItem>>(new Exception("Erro Intero em algum dos Processos")),
                error => new Result<IEnumerable<ExplorerItem>>(error));

    public async Task<Result<IEnumerable<ExplorerItem>>> GetAllItemsAsync(
        string solutionPath
    ) =>
        OptionalResult<string>.Optional(solutionPath)
            .Map(path =>
            {
                return File.GetAttributes(solutionPath) switch
                {
                    FileAttributes.Archive => Path.Combine(
                        Directory.GetParent(solutionPath)
                            .ToOption()
                            .Map(item => item.FullName)
                            .Reduce(() => ""),
                        Constants.AppProjectItemsFolderName)
                    , FileAttributes.Directory => Path.Combine(
                        solutionPath,
                        Constants.AppProjectItemsFolderName)
                    , _ => ""
                };
            })
            .Map(storageItem =>
            {
                var items = Enumerable.Empty<ExplorerItem>();

                if (Directory.Exists(storageItem))
                {
                    var folderItem = new FolderItem(Guid.NewGuid())
                    {
                        Name = Path.GetDirectoryName(storageItem)
                        , Path = storageItem, Children = new List<ExplorerItem>()
                    };

                    items = items.Append(folderItem);

                    var directory = Directory.GetFileSystemEntries(storageItem);

                    GetDataFromPath(
                        directory,
                        folderItem.Children);
                }

                return items;
            })
            .Match(resultItem =>
                {
                    if (resultItem.Any())
                    {
                        return new Result<IEnumerable<ExplorerItem>>(resultItem);
                    }

                    return new Result<IEnumerable<ExplorerItem>>(new Exception("Não Há items"));
                },
                () => new Result<IEnumerable<ExplorerItem>>(new Exception("Erro Intero em algum dos Processos")),
                error => new Result<IEnumerable<ExplorerItem>>(error));

    public Result<ExplorerItem> UpdateExplorerItem(
        ExplorerItem item
    ) => new(new ExplorerItem(Guid.NewGuid()));

    public async Task<Result<ExplorerItem>> UpdateExplorerItemAsync(
        ExplorerItem item
    ) => new(new ExplorerItem(Guid.NewGuid()));

    public async Task<Result<ExplorerItem>> RenameFileItemAsync(
        ExplorerItem item
    ) =>
        (await OptionalResult<ExplorerItem>.Optional(item)
            .Map(explorerItem =>
                (itemName: explorerItem.Name, itemPath: explorerItem.Path
                    , exists: File.Exists(explorerItem.Path)))
            .MapAsync(async explorerItem =>
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        if (explorerItem.exists && Directory.GetParent(item.Path) is
                                { Exists: true } info)
                        {
                            File.Move(
                                explorerItem.itemPath,
                                Path.Combine(
                                    info!.FullName,
                                    $"{item.Name}{Constants.AppProjectItemExtension}"));

                            item.Path = explorerItem.itemPath.Replace(
                                Path.GetFileName(explorerItem.itemPath),
                                $"{item.Name}{Constants.AppProjectItemExtension}");

                            return new Result<ExplorerItem>(item);
                        }

                        return new Result<ExplorerItem>(new Exception("Erro na operação!"));
                    }
                    catch (IOException ex)
                    {
                        return new Result<ExplorerItem>(ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        return new Result<ExplorerItem>(ex);
                    }
                    catch (ArgumentException ex)
                    {
                        return new Result<ExplorerItem>(ex);
                    }
                    catch (NotSupportedException ex)
                    {
                        return new Result<ExplorerItem>(ex);
                    }
                    catch (SecurityException ex)
                    {
                        return new Result<ExplorerItem>(ex);
                    }
                });
            }))
        .Match(item => new Result<ExplorerItem>(),
            () => new Result<ExplorerItem>(new Exception("Erro na operação, valor vazio")),
            error => new Result<ExplorerItem>(error));

    public async Task<Result<string>> RenameSystemItemAsync(
        string name
        , string path
    ) =>
        (await OptionalResult<string>.Optional(path)
            .Map(explorerItem =>
                (itemName: name, itemPath: path
                    , exists: File.Exists(path)))
            .MapAsync(async explorerItem =>
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        if (explorerItem.exists && Directory.GetParent(path) is
                                { Exists: true } info)
                        {
                            File.Move(
                                explorerItem.itemPath,
                                Path.Combine(
                                    info!.FullName,
                                    $"{name}{Constants.AppProjectItemExtension}"));

                            path = explorerItem.itemPath.Replace(
                                Path.GetFileName(explorerItem.itemPath),
                                $"{name}{Constants.AppProjectItemExtension}");

                            return new Result<string>(path);
                        }

                        return new Result<string>(new Exception("Erro na operação!"));
                    }
                    catch (IOException ex)
                    {
                        return new Result<string>(ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        return new Result<string>(ex);
                    }
                    catch (ArgumentException ex)
                    {
                        return new Result<string>(ex);
                    }
                    catch (NotSupportedException ex)
                    {
                        return new Result<string>(ex);
                    }
                    catch (SecurityException ex)
                    {
                        return new Result<string>(ex);
                    }
                });
            }))
        .Match(item => new Result<string>(),
            () => new Result<string>(new Exception("Erro na operação, valor vazio")),
            error => new Result<string>(error));


    public async Task<Result<ExplorerItem>> RenameFolderItemAsync(
        ExplorerItem item
    )
    {
        if (item is not null)
        {
            if (Directory.Exists(item.Path))
            {
                Directory.Move(
                    item.Path,
                    Path.Combine(
                        Directory.GetParent(item.Path)
                            .FullName,
                        item.Name));
            }
            else
            {
                _ = Directory.CreateDirectory(
                    Path.Combine(
                        item.Path,
                        item.Name));
            }

            item.Path = item.Path.Replace(
                Path.GetFileName(item.Path),
                item.Name);
        }

        return new Result<ExplorerItem>(item);
    }

    public async Task<Result<ExplorerItem>> DeleteFolderItemAsync(
        string itemPath
    )
    {
        if (itemPath is not null)
        {
            if (Directory.Exists(itemPath))
            {
                Directory.Delete(itemPath, true);
                return new Result<ExplorerItem>(new ExplorerItem(Guid.NewGuid()));
            }
        }

        return new Result<ExplorerItem>(new Exception("Diretório não existe"));
    }

    public async Task<Result<ExplorerItem>> DeleteFileItemAsync(
        string itemPath
    )
    {
        if (itemPath is not null)
        {
            if (File.Exists(itemPath))
            {
                await Task.Run(() =>
                {
                    File.Delete(itemPath);
                });

                return new Result<ExplorerItem>(new ExplorerItem(Guid.NewGuid()));
            }
        }

        return new Result<ExplorerItem>(new Exception("Erro ao deletar arquivo"));
    }

    public Result<ExplorerItem> RenameFolderItem(
        ExplorerItem item
    )
    {
        var path = Directory.GetParent(item.Path)
            .FullName;
        if (item is not null)
        {
            if (File.Exists(item.Path))
            {
                File.Move(
                    item.Path,
                    Path.Combine(
                        path,
                        $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(
                    item.ReferencedItem,
                    Path.Combine(
                        path,
                        $"{item.Name}{Constants.AppProjectItemExtension}"));
            }

            item.Path = item.Path.Replace(
                Path.GetFileName(item.Path),
                $"{item.Name}{Constants.AppProjectItemExtension}");
        }

        return new Result<ExplorerItem>(new ExplorerItem(Guid.NewGuid()));
    }

    public Result<ExplorerItem> DeleteFileItem(
        ExplorerItem item
    )
    {
        var path = Directory.GetParent(item.Path)
            .FullName;
        if (item is not null)
        {
            if (File.Exists(item.Path))
            {
                File.Move(
                    item.Path,
                    Path.Combine(
                        path,
                        $"{item.Name}{Constants.AppProjectItemExtension}"));
            }
            else
            {
                File.Copy(
                    item.ReferencedItem,
                    Path.Combine(
                        path,
                        $"{item.Name}{Constants.AppProjectItemExtension}"));
            }

            item.Path = item.Path.Replace(
                Path.GetFileName(item.Path),
                $"{item.Name}{Constants.AppProjectItemExtension}");
        }

        return new Result<ExplorerItem>(new ExplorerItem(Guid.NewGuid()));
    }

    public async Task<Result<ExplorerItem>> CreateExplorerItemAsync(
        ExplorerItem item
    ) => new(new ExplorerItem(Guid.NewGuid()));

    public async Task<Result<ExplorerItem>> DeleteExplorerItemAsync(
        ExplorerItem item
    ) => new(new ExplorerItem(Guid.NewGuid()));

    public Result<Empty> MoveFileItem(
        string oldPath
        , string newPath
    ) =>
        OptionalResult<string>.Optional(oldPath)
            .Match(item =>
                {
                    try
                    {
                        if (File.Exists(item))
                        {
                            if (!string.IsNullOrWhiteSpace(newPath))
                            {
                                File.Move(item, newPath);
                                return new Result<Empty>(Empty.Value);
                            }

                            return new Result<Empty>(
                                new Exception("O destino do arquivo não pode ser nulo"));
                        }

                        return new Result<Empty>(new Exception("O arquivo não existe"));
                    }
                    catch (IOException ex)
                    {
                        return new Result<Empty>(ex);
                    }
                },
                () => new Result<Empty>(new Exception("A Operação não pode ser concluída")),
                error => new Result<Empty>(error));

    public Result<Empty> RenameFolderItem(
        string itemName
        , string itemPath
    )
    {
        if (!string.IsNullOrWhiteSpace(itemName) &&
            !string.IsNullOrWhiteSpace(itemPath))
        {
            if (Directory.Exists(itemPath))
            {
                if (Path.Combine(
                        Directory.GetParent(itemPath)
                            .FullName,
                        itemName) !=
                    itemPath)
                {
                    Directory.Move(
                        itemPath,
                        Path.Combine(
                            Directory.GetParent(itemPath)
                                .FullName,
                            itemName));
                }
            }
            else
            {
                _ = Directory.CreateDirectory(itemPath);
            }
        }

        return new Result<Empty>(new Empty());
    }

    private async Task GetDataFromPathAsync(
        string[] folder
        , IList<ExplorerItem> list
    )
    {
        foreach (var item in folder)
        {
            if (File.GetAttributes(item)
                .HasFlag(FileAttributes.Archive))
            {
                var i = new FileItem(Guid.NewGuid())
                {
                    Name = Path.GetFileNameWithoutExtension(item), Path = item
                };
                list.Add(i);
            }

            if (File.GetAttributes(item)
                .HasFlag(FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(item);

                var folderItem = new FolderItem(Guid.NewGuid())
                {
                    Name = Path.GetFileNameWithoutExtension(item), Path = item
                };

                foreach (var fileItem in itens)
                {
                    if (File.GetAttributes(fileItem)
                        .HasFlag(FileAttributes.Directory))
                    {
                        var entries = Directory.GetFileSystemEntries(fileItem);

                        await GetDataFromPathAsync(
                            entries,
                            folderItem.Children);
                    }

                    if (File.GetAttributes(fileItem)
                        .HasFlag(FileAttributes.Archive))
                    {
                        var i = new FileItem(Guid.NewGuid())
                        {
                            Name = Path.GetFileNameWithoutExtension(fileItem), Path = fileItem
                        };
                        folderItem.Children.Add(i);
                    }
                }

                list.Add(folderItem);
            }
        }
    }

    private void GetDataFromPath(
        string[] folder
        , IList<ExplorerItem> list
    )
    {
        foreach (var item in folder)
        {
            var fileAttributes = File.GetAttributes(item);

            if (File.GetAttributes(item)
                .HasFlag(FileAttributes.Archive))
            {
                var i = new FileItem(Guid.NewGuid())
                {
                    Name = Path.GetFileNameWithoutExtension(item), Path = item
                };
                list.Add(i);
            }

            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(item);

                var folderItem = new FolderItem(Guid.NewGuid())
                {
                    Name = Path.GetDirectoryName(item), Path = item
                };

                foreach (var fileItem in itens)
                {
                    if (fileAttributes.HasFlag(FileAttributes.Directory))
                    {
                        var entries = Directory.GetFileSystemEntries(fileItem);

                        GetDataFromPath(
                            entries,
                            folderItem.Children);
                    }

                    if (File.GetAttributes(item)
                        .HasFlag(FileAttributes.Archive))
                    {
                        var i = new FileItem(Guid.NewGuid())
                        {
                            Name = Path.GetFileNameWithoutExtension(item), Path = item
                        };
                        folderItem.Children.Add(i);
                    }
                }

                list.Add(folderItem);
            }
        }
    }
}