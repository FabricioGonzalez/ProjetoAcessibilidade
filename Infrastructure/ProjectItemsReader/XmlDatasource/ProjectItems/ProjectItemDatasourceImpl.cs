using System.Xml.Serialization;
using LanguageExt.Common;
using XmlDatasource.ProjectItems.DTO;

namespace XmlDatasource.ProjectItems;

public class ProjectItemDatasourceImpl
{
    private readonly XmlSerializer _serializer = new(typeof(ItemRoot));

    public Task SaveContentItem(
        string path
        , ItemRoot item
    ) =>
        Task.CompletedTask;

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
            }

            return new Result<ItemRoot>(new InvalidOperationException(""));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<ItemRoot>(e);
        }
    }
}