using App.Core.Entities.Solution.Project.AppItem;

namespace Project.Application.Project.Contracts;
public interface IProjectItemContentRepository
{
    public Task<AppItemModel> GetProjectItemContent(string filePathToWrite);
    public Task SaveProjectItemContent(AppItemModel dataToWrite, string filePathToWrite);
}
