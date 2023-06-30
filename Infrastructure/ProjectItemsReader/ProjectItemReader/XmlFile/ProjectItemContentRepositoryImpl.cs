using System.Xml;
using System.Xml.Serialization;

using Common.Optional;
using Common.Result;

using Core.Entities.Solution.Project.AppItem;

using ProjectItemReader.InternalAppFiles.DTO;
using ProjectItemReader.XmlFile.DTO;

using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjectItemReader.XmlFile;

public class ProjectItemContentRepositoryImpl : IProjectItemContentRepository
{
    public XmlSerializer CreateSerealizer() => _serializer ??= new(type: typeof(ItemRoot));
    private static XmlSerializer _serializer;

    public async Task<Result<AppItemModel>> GetSystemProjectItemContent(
        string filePathToRead
    )
    {
        return await Task.Run(() =>
        {
            return filePathToRead
            .ToOption()
            .MapValue(item =>
            {
                try
                {
                    if (Path.Exists(path: filePathToRead))
                    {
                        return CreateSerealizer()
                    .ToOption()
                    .MapValue(res =>
                    {
                        using var reader = XmlReader.Create(inputUri: filePathToRead);

                        if (res.Deserialize(xmlReader: reader) is { } result)
                        {
                            return Result<ItemRoot>.Success((ItemRoot)result);
                        }

                        return Result<ItemRoot>.Failure(new Exception("Não foi possível ler item"));
                    })
                    .Reduce(() => Result<ItemRoot>.Failure(new Exception("Impossível de completar leitura")));
                    }

                    return Result<ItemRoot>.Failure(new Exception("Item inexistente"));
                }
                catch (Exception ex)
                {
                    return Result<ItemRoot>.Failure(ex);
                }
            })
            .Reduce(() => Result<ItemRoot>.Failure(new Exception("Item inexistente")))
            .Match(success => Result<AppItemModel>.Success(success.ToAppItemModel()), Result<AppItemModel>.Failure);

        });
    }

    public async Task<Result<AppItemModel>> GetProjectItemContent(
        string filePathToRead
    )
    {
        return (await filePathToRead
             .ToOption()
             .Map(async item =>
             {
                 try
                 {
                     if (File.Exists(item))
                     {
                         var serializer = new XmlSerializer(type: typeof(ItemRoot));

                         using var reader = XmlReader.Create(inputUri: filePathToRead);

                         return Result<ItemRoot>.Success(await Task.Run<ItemRoot>(function: () => (ItemRoot)serializer.Deserialize(xmlReader: reader)));
                     }
                     return Result<ItemRoot>.Failure(new Exception("Arquivo inexistente"));
                 }
                 catch (Exception ex)
                 {
                     return Result<ItemRoot>.Failure(ex);
                 }
             })
             .Reduce(() => Task.Run(() => Result<ItemRoot>.Failure(new Exception("Arquivo inexistente"))))
             )
             .Match(Succ: success => Result<AppItemModel>.Success(success.ToAppItemModel()),
             Fail: Result<AppItemModel>.Failure);
    }

    public async Task SaveProjectItemContent(
        AppItemModel dataToWrite
        , string filePathToWrite
    )
    {
        var serializer = new XmlSerializer(type: typeof(ItemRoot));

        using var writer = new StreamWriter(stream: File.Create(path: filePathToWrite));

        serializer.Serialize(textWriter: writer, o: dataToWrite.ToItemRoot());
    }

    public async Task SaveSystemProjectItemContent(
        AppItemModel dataToWrite
        , string filePathToWrite
    )
    {
        var serializer = new XmlSerializer(type: typeof(ItemRoot));

        using var writer = new StreamWriter(stream: File.Create(path: filePathToWrite));

        serializer.Serialize(textWriter: writer, o: dataToWrite.ToItemRoot());
    }

    public async Task<Result<AppItemModel>> GetSystemProjectItemContentSerealizer(
        string filePathToRead
    )
    {
        return filePathToRead
            .ToOption()
            .MapValue(itemPath =>
            {
                try
                {
                    if (Path.Exists(path: filePathToRead))
                    {
                        var serializer = new XmlSerializer(type: typeof(ItemRoot));

                        using var reader = XmlReader.Create(inputUri: filePathToRead);
                        var result = serializer.Deserialize(xmlReader: reader);

                        return Result<ItemRoot>.Success((ItemRoot)result);
                    }
                    return Result<ItemRoot>.Failure(new Exception("File does not exists"));
                }
                catch (Exception ex)
                {
                    return Result<ItemRoot>.Failure(ex);
                }
            })
            .MapValue(item =>
            {
                return item.Match(
                    success => Result<AppItemModel>.Success(success.ToAppItemModel()),
                    fail => Result<AppItemModel>.Failure(new Exception("File was not returned")));
            })
            .Reduce(() => Result<AppItemModel>.Failure(new Exception("Process has broken")));


    }

    public async Task SaveSystemProjectItemContentSerealizer(
        AppItemModel dataToWrite
        , string filePathToWrite
    )
    {
        var serializer = new XmlSerializer(type: typeof(ItemRoot));

        using var writer = new StreamWriter(stream: File.Create(path: filePathToWrite));

        serializer.Serialize(textWriter: writer, o: dataToWrite.ToItemRoot());
    }
}