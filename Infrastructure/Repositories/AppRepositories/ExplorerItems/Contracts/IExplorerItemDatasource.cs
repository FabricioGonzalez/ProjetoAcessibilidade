using AppRepositories.ExplorerItems.Dto;
using LanguageExt.Common;

namespace AppRepositories.ExplorerItems.Contracts;

public interface IExplorerItemDatasource
{
    public Result<IEnumerable<AppTemplateDto>> GetAllAppTemplates();

    public Result<IEnumerable<ProjectTemplateDto>> GetProjectTemplate(
        string solutionPath
    );
}