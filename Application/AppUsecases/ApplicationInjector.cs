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
        services.AddTransient<IQueryUsecase<object, ProjectSolutionModel>, GetProjectSolutionUsecase>();

        services.AddTransient<ICommandUsecase<object, ProjectSolutionModel>, CreateProjectSolutionUsecase>();
    } 
}
