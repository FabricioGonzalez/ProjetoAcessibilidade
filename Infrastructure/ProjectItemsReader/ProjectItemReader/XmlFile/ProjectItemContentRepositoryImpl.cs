using System.Xml;
using System.Xml.Serialization;

using Common.Optional;

using Core.Entities.Solution.Project.AppItem;

using ProjectItemReader.XmlFile.DTO;

using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjectItemReader.XmlFile;

public class ProjectItemContentRepositoryImpl : IProjectItemContentRepository
{
    public XmlSerializer CreateSerealizer() => new(type: typeof(ItemRoot));

    public async Task<Optional<AppItemModel>> GetSystemProjectItemContent(
        string filePathToRead
    )
    {
        return await Task.Run(() =>
        {
            if (!Path.Exists(path: filePathToRead))
            {
                Optional<AppItemModel>.None();
            }

            return CreateSerealizer()
                .ToOption()
            .Map(res =>
            {
                using var reader = XmlReader.Create(inputUri: filePathToRead);

                return res.Deserialize(xmlReader: reader)
                .ToOption()
                .Map(res => ((ItemRoot)res).ToAppItemModel());
            })
                .Reduce(() => Optional<AppItemModel>.None());
        });
    }

    public async Task<AppItemModel?> GetProjectItemContent(
        string filePathToRead
    )
    {
        if (!Path.Exists(path: filePathToRead))
        {
            return null;
        }

        var serializer = new XmlSerializer(type: typeof(ItemRoot));

        using var reader = XmlReader.Create(inputUri: filePathToRead);

        var result = await Task.Run<ItemRoot>(function: () => (ItemRoot)serializer.Deserialize(xmlReader: reader));

        return result.ToAppItemModel();
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

    public async Task<AppItemModel?> GetSystemProjectItemContentSerealizer(
        string filePathToRead
    )
    {
        if (!Path.Exists(path: filePathToRead))
        {
            return null;
        }

        var serializer = new XmlSerializer(type: typeof(ItemRoot));

        using var reader = XmlReader.Create(inputUri: filePathToRead);

        var result = await Task.Run<ItemRoot>(function: () => (ItemRoot)serializer.Deserialize(xmlReader: reader));

        return result.ToAppItemModel();
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