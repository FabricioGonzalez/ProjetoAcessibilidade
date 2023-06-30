using Common.Result;

using Core.Entities.Solution.Project.AppItem;

namespace ProjetoAcessibilidade.Domain.Project.Contracts;

public interface IProjectItemContentRepository
{
    public Task<Result<AppItemModel>> GetProjectItemContent(
        string filePathToWrite
    );

    public Task<Result<AppItemModel>> GetSystemProjectItemContent(
        string filePathToWrite
    );

    public Task<Result<AppItemModel>> GetSystemProjectItemContentSerealizer(
        string filePathToWrite
    );

    public Task SaveProjectItemContent(
        AppItemModel dataToWrite
        , string filePathToWrite
    );

    public Task SaveSystemProjectItemContent(
        AppItemModel dataToWrite
        , string filePathToWrite
    );

    public Task SaveSystemProjectItemContentSerealizer(
        AppItemModel dataToWrite
        , string filePathToWrite
    );
}