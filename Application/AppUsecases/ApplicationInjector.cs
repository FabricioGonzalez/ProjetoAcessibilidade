using System.Collections.Generic;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities;
using AppUsecases.Entities.FileTemplate;
using AppUsecases.Usecases;

using Microsoft.Extensions.DependencyInjection;

namespace AppUsecases;
public static class ApplicationInjector
{
    public static void Inject(IServiceCollection services)
    {
        services.AddTransient<IQueryUsecase<string, List<ExplorerItem>>, GetProjectItemsUsecase>();
        services.AddTransient<IQueryUsecase<ProjectSolutionModel>, GetProjectSolutionUsecase>();
        services.AddTransient<IQueryUsecase<List<FileTemplate>>, GetProjectTemplateUsecase>();
        services.AddTransient<IQueryUsecase<string, AppItemModel>, GetProjectItemContentUsecase>();

        services.AddTransient<ICommandUsecase<ProjectSolutionModel>, CreateProjectSolutionUsecase>();
    } 
}
