using System.Xml;
using System.Xml.Serialization;
using Common.Optional;
using Core.Entities.Solution.Project.AppItem;
using LanguageExt;
using LanguageExt.Common;
using ProjectItemReader.XmlFile.DTO;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjectItemReader.XmlFile;

public class ProjectItemContentRepositoryImpl : IProjectItemContentRepository
{
    private static XmlSerializer _serializer;

    public async Task SaveProjectItemContent(
        AppItemModel dataToWrite
        , string filePathToWrite
    )
    {
        var serializer = new XmlSerializer(typeof(ItemRoot));

        using var writer = new StreamWriter(File.Create(filePathToWrite));

        serializer.Serialize(writer, dataToWrite.ToItemRoot());
    }

    public async Task SaveSystemProjectItemContent(
        AppItemModel dataToWrite
        , string filePathToWrite
    )
    {
        var serializer = new XmlSerializer(typeof(ItemRoot));

        using var writer = new StreamWriter(File.Create(filePathToWrite));

        serializer.Serialize(writer, dataToWrite.ToItemRoot());
    }

    public async Task SaveSystemProjectItemContentSerealizer(
        AppItemModel dataToWrite
        , string filePathToWrite
    )
    {
        var serializer = new XmlSerializer(typeof(ItemRoot));

        using var writer = new StreamWriter(File.Create(filePathToWrite));

        serializer.Serialize(writer, dataToWrite.ToItemRoot());
    }

    public async Task<Result<AppItemModel>> GetSystemProjectItemContent(
        string filePathToRead
    ) =>
        await Task.Run(() =>
        {
            return Option<string>.Some(filePathToRead)
                .Map(item =>
                {
                    try
                    {
                        if (Path.Exists(filePathToRead))
                        {
                            return CreateSerealizer()
                                .ToOption()
                                .MapValue(res =>
                                {
                                    using var reader = XmlReader.Create(filePathToRead);

                                    if (res.Deserialize(reader) is { } result)
                                    {
                                        return new Result<ItemRoot>((ItemRoot)result);
                                    }

                                    return new Result<ItemRoot>(new Exception("Não foi possível ler item"));
                                })
                                .Reduce(() =>
                                    new Result<ItemRoot>(new Exception("Impossível de completar leitura")));
                        }

                        return new Result<ItemRoot>(new Exception("Item inexistente"));
                    }
                    catch (Exception ex)
                    {
                        return new Result<ItemRoot>(ex);
                    }
                })
                .Map(item =>
                {
                    return item.Match(success => new Result<AppItemModel>(success.ToAppItemModel())
                        , fail => new Result<AppItemModel>(fail));
                })
                .Match(some => some,
                    () => new Result<AppItemModel>(new Exception("Impossível de completar leitura")));
        });

    public async Task<Result<AppItemModel>> GetProjectItemContent(
        string filePathToRead
    ) =>
        await Option<string>.Some(filePathToRead)
            .MapAsync(async item =>
            {
                try
                {
                    if (File.Exists(item))
                    {
                        var serializer = new XmlSerializer(typeof(ItemRoot));

                        using var reader = XmlReader.Create(filePathToRead);

                        return new Result<ItemRoot>(await Task.Run<ItemRoot>(() =>
                            (ItemRoot)serializer.Deserialize(reader)));
                    }

                    return new Result<ItemRoot>(new Exception("Arquivo inexistente"));
                }
                catch (Exception ex)
                {
                    return new Result<ItemRoot>(ex);
                }
            })
            .MatchAsync(result => Task.Run(() =>
                {
                    return result.Match(success =>
                            new Result<AppItemModel>(success.ToAppItemModel()),
                        error => new Result<AppItemModel>(error));
                }),
                () => new Result<AppItemModel>(new Exception("Impossível de completar leitura")));

    public async Task<Result<AppItemModel>> GetSystemProjectItemContentSerealizer(
        string filePathToRead
    ) =>
        await Option<string>.Some(filePathToRead)
            .MapAsync(itemPath =>
            {
                try
                {
                    if (Path.Exists(filePathToRead))
                    {
                        var serializer = new XmlSerializer(typeof(ItemRoot));

                        using var reader = XmlReader.Create(filePathToRead);
                        var result = serializer.Deserialize(reader);

                        return new Result<ItemRoot>((ItemRoot)result);
                    }

                    return new Result<ItemRoot>(new Exception("File does not exists"));
                }
                catch (Exception ex)
                {
                    return new Result<ItemRoot>(ex);
                }
            })
            .MatchAsync(result => Task.Run(() =>
                {
                    return result.Match(
                        success => new Result<AppItemModel>(success.ToAppItemModel()),
                        fail => new Result<AppItemModel>(new Exception("File was not returned")));
                }),
                () => new Result<AppItemModel>(new Exception("Impossível de completar leitura")));

    public XmlSerializer CreateSerealizer() => _serializer ??= new XmlSerializer(typeof(ItemRoot));
}