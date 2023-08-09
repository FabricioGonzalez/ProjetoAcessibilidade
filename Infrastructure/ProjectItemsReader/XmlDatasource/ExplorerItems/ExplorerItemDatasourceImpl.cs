using AppRepositories.ExplorerItems.Contracts;
using AppRepositories.ExplorerItems.Dto;
using Common;
using Domain.Explorer.Exceptions;
using LanguageExt.Common;

namespace XmlDatasource.ExplorerItems;

public class ExplorerItemDatasourceImpl : IExplorerItemDatasource
{
    public Result<IEnumerable<AppTemplateDto>> GetAllAppTemplates()
    {
        try
        {
            var result = Directory.GetFiles(path: Constants.AppItemsTemplateFolder
                    , searchPattern: $"*.{Constants.AppProjectTemplateExtension}")
                .Select(it => new AppTemplateDto(Name: it.Split("%")[0], TemplateName: it.Split("%")[1]
                    , Version: Convert.ToInt32(it.Split("%")[2])))
                .ToList();

            if (result.Any())
            {
                return new Result<IEnumerable<AppTemplateDto>>(result);
            }

            return new Result<IEnumerable<AppTemplateDto>>(ExplorerExceptions.NoItemsWasFoundException);
        }
        catch (Exception e)
        {
            return new Result<IEnumerable<AppTemplateDto>>(e);
        }
    }

    public Result<IEnumerable<ProjectTemplateDto>> GetProjectTemplate(
        string solutionPath
    )
    {
        try
        {
            return new Result<IEnumerable<ProjectTemplateDto>>(Enumerable.Empty<ProjectTemplateDto>());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}