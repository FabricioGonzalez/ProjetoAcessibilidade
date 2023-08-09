using System.Xml.Serialization;
using AppRepositories.ProjectItems.Contracts;
using AppRepositories.ProjectItems.Dto;
using Domain.FormBody.Exceptions;
using LanguageExt.Common;
using XmlDatasource.XmlFile.DTO;

namespace XmlDatasource.ProjectItems;

public class ProjectItemDatasourceImpl : IProjectItemDatasource
{
    private readonly XmlSerializer _serializer = new(typeof(ItemRoot));

    public Task SaveContentItem(
        string path
        , RootItem item
    ) =>
        Task.CompletedTask;

    public async Task<Result<RootItem>> GetContentItem(
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

            return new Result<RootItem>(FormBodyExceptions.ItemIsNotAFileException);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<RootItem>(e);
        }
    }
}