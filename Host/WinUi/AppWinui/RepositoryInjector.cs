using AppUsecases.Contracts.Repositories;
using AppUsecases.Editing.Entities;
using AppUsecases.Project.Entities.Project;
using Common;
using WindowsLocalRepository.FileRepository.Repository.InternalAppFiles;
using Microsoft.Extensions.DependencyInjection;

using ProjectItemReader.XmlFile;
using AppUsecases.Project.Entities.FileTemplate;

namespace AppWinui;
public static class RepositoryInjector
{
    public static void Inject(IServiceCollection services)
    {
        services.AddTransient<IReadContract<ProjectSolutionModel>, ReadUserProjectSolutionFileRepository>();
        services.AddTransient<IReadContract<List<ExplorerItem>>, ReadAllUserProjectTemplateFilesRepository>();
        services.AddTransient<IReadContract<Resource<List<FileTemplate>>>, ReadAllProjectTemplateFilesRepository>();
        services.AddTransient<IReadContract<AppItemModel>, ReadXmlFileRepository>();
    }
}
