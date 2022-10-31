using System.Collections.Generic;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Editing.Entities;
using AppUsecases.Entities.FileTemplate;
using AppUsecases.Project.Entities.Project;
using Common;

using LocalRepository.FileRepository.Repository.InternalAppFiles;
using LocalRepository.FileRepository.Repository.XmlFile;

using Microsoft.Extensions.DependencyInjection;

namespace LocalRepository;
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
