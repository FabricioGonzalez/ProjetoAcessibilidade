﻿using System.Collections.Generic;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Entities;
using AppUsecases.Entities.FileTemplate;

using LocalRepository.FileRepository.Repository.InternalAppFiles;

using Microsoft.Extensions.DependencyInjection;

namespace LocalRepository;
public static class RepositoryInjector
{
    public static void Inject(IServiceCollection services)
    {
        services.AddTransient<IReadContract<ProjectSolutionModel>, ReadUserProjectSolutionFileRepository>();
        services.AddTransient<IReadContract<List<ExplorerItem>>, ReadAllUserProjectTemplateFilesRepository>();
    }
}
