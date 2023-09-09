using Common;
using LanguageExt.Common;
using XmlDatasource.ExplorerItems.Dto;

namespace XmlDatasource.ExplorerItems;

public sealed class XmlExplorerItemDatasourceImpl
{
    public Result<IEnumerable<AppTemplateDto>> GetAllAppTemplates()
    {
        try
        {
            var result = Directory.GetFiles(path: Constants.AppItemsTemplateFolder
                    , searchPattern: $"*{Constants.AppProjectTemplateExtension}")
                .Select(it => new AppTemplateDto(Name: Path.GetFileNameWithoutExtension(it)
                    , TemplateName: Path.GetFileNameWithoutExtension(it), Version: 0))
                .ToList();

            if (result.Any())
            {
                return new Result<IEnumerable<AppTemplateDto>>(result);
            }

            return new Result<IEnumerable<AppTemplateDto>>(new IOException("No file was found"));
        }
        catch (Exception e)
        {
            return new Result<IEnumerable<AppTemplateDto>>(e);
        }
    }

    /*public Result<IEnumerable<ProjectTemplateDto>> GetProjectTemplate(
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
    }*/
}