using AppRepositories.ProjectItems.Dto;
using LanguageExt.Common;

namespace AppRepositories.ProjectItems.Contracts;

public interface IProjectItemDatasource
{
    public Task<Result<RootItem>> GetContentItem(
        string path
    );

    public Task SaveContentItem(
        string path
        , RootItem item
    );
}