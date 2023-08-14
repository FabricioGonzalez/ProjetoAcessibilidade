using System.Text;
using System.Xml.Serialization;
using LanguageExt.Common;
using XmlDatasource.ProjectItems.DTO;

namespace XmlDatasource.ProjectItems;

public sealed class ProjectItemDatasourceImpl
{
    private readonly XmlSerializer _serializer = new(typeof(ItemRoot));

    public async Task SaveContentItem(
        string path
        , ItemRoot item
    ) =>
        await Task.Run(() =>
        {
            try
            {
                using var writer = new StreamWriter(path);
                _serializer.Serialize(textWriter: writer, o: item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });

    public async Task<Result<ItemRoot>> GetContentItem(
        string path
    )
    {
        try
        {
            if (path is { Length: > 0 }
                && Path.Exists(path)
                && !File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                using var reader = new StreamReader(path);
                if (_serializer.Deserialize(reader) is { } result)
                {
                    return new Result<ItemRoot>((ItemRoot)result);
                }

                return new Result<ItemRoot>(new InvalidOperationException($"Erro ao Deserializar {path}"));
            }

            return new Result<ItemRoot>(new InvalidOperationException(""));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<ItemRoot>(e);
        }
    }

    public async Task SaveConclusionItem(
        string conclusionItemPath
        , string conclusionBody
    )
    {
        using var writer = new StreamWriter(path: conclusionItemPath, encoding: Encoding.UTF8
            , options: new FileStreamOptions { Mode = FileMode.Create, Access = FileAccess.Write });
        {
            await writer.WriteAsync(conclusionBody);
        }
    }
}