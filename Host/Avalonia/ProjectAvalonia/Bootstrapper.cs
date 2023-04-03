using System.Collections.Generic;
using System.IO;

using Avalonia;

using Common;

using Core.Entities.App;
using Core.Entities.Solution;
using Core.Entities.Solution.Project.AppItem;

using Microsoft.Extensions.Configuration;

using Project.Domain.App.Contracts;
using Project.Domain.App.Models;
using Project.Domain.App.Queries.GetAllTemplates;
using Project.Domain.App.Queries.GetUFList;
using Project.Domain.Contracts;
using Project.Domain.Implementations;
using Project.Domain.Project.Commands.ProjectItemCommands.CreateItemCommands;
using Project.Domain.Project.Commands.ProjectItemCommands.DeleteCommands;
using Project.Domain.Project.Commands.ProjectItemCommands.RenameCommands;
using Project.Domain.Project.Commands.ProjectItemCommands.SaveCommands;
using Project.Domain.Project.Contracts;
using Project.Domain.Project.Queries.GetProjectItemContent;
using Project.Domain.Project.Queries.GetProjectItems;
using Project.Domain.Solution.Commands.CreateSolutionCommands;
using Project.Domain.Solution.Commands.SyncSolutionCommands;
using Project.Domain.Solution.Contracts;
using Project.Domain.Solution.Queries;

using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Common.Services;

using ProjectItemReader.InternalAppFiles;
using ProjectItemReader.XmlFile;

using Splat;

using Constants = Common.Constants;
using ExplorerItem = Core.Entities.Solution.Explorer.ExplorerItem;

namespace ProjectAvalonia;
public static class Bootstrapper
{
    /* public static IMutableDependencyResolver AddViewModel(this IMutableDependencyResolver service)
     {
         service.RegisterLazySingleton(() => new MainViewModel());

         service.RegisterLazySingleton(() => new TemplateEditingViewModel());

         service.RegisterLazySingleton(() => new TemplateRulesViewModel());

         service.RegisterLazySingleton(() => new SolutionStateViewModel());

         service.RegisterLazySingleton(() => new SettingsViewModel());

         service.RegisterLazySingleton(() => new ProjectViewModel());

         service.RegisterLazySingleton(() => new ProjectEditingViewModel());

         service.RegisterLazySingleton(() => new PreviewerViewModel());

         service.RegisterLazySingleton(() => new TemplateEditingPageViewModel());

         service.RegisterLazySingleton(() => new ProjectExplorerViewModel());

         service.RegisterLazySingleton(() => new ProjectItemEditingViewModel());

         return service;
     }*/

    /*    public static IMutableDependencyResolver AddViewComponents(this IMutableDependencyResolver service)
        {
            service.Register(() => new ExplorerComponent());

            service.Register(() => new PreviewerPage());

            service.RegisterLazySingleton(() => new MainWindow());

            service.Register(() => new AddItemWindow());

            return service;
        }*/

    /*public static IMutableDependencyResolver AddViewModelOperations(this IMutableDependencyResolver service)
    {
        service.RegisterLazySingleton(() => new ProjectExplorerOperations());

        return service;
    }*/

    public static AppBuilder AddQueryHandlers(this AppBuilder app)
    {
        var service = Locator.CurrentMutable;

        service
            .Register(() => new GetProjectItemsQueryHandler(
     Locator.Current.GetService<IExplorerItemRepository>()!
    ));
        service
            .Register<IQueryHandler<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>>(() => new ReadSolutionProjectQueryHandler(
     Locator.Current.GetService<ISolutionRepository>()!
    ));

        service
            .Register<IQueryHandler<GetAllUFQuery, IList<UFModel>>>(() => new GetAllUFQueryHandler());

        service
            .Register<IQueryHandler<GetProjectItemContentQuery, Resource<AppItemModel>>>(() => new GetProjectItemContentQueryHandler(
                Locator.Current.GetService<IProjectItemContentRepository>()!));
        service
            .Register<IQueryHandler<GetSystemProjectItemContentQuery, Resource<AppItemModel>>>(() => new GetSystemProjectItemContentQueryHandler(
                Locator.Current.GetService<IProjectItemContentRepository>()!));

        service
            .Register<IQueryHandler<GetAllTemplatesQuery, Resource<List<ExplorerItem>>>>(() => new GetAllTemplatesQueryHandler(
                Locator.Current.GetService<IAppTemplateRepository>()!));

        service
            .Register<IQueryHandler<GetProjectItemsQuery, Resource<List<ExplorerItem>>>>(() => new GetProjectItemsQueryHandler(
                Locator.Current.GetService<IExplorerItemRepository>()!));

        return app;
    }
    public static AppBuilder AddCommandHandlers(this AppBuilder app)
    {
        var service = Locator.CurrentMutable;

        service.Register<ICommandHandler<SaveProjectItemContentCommand, Resource<Empty>>>(() => new SaveProjectItemContentCommandHandler(
          Locator.Current.GetService<IProjectItemContentRepository>()!));

        service.Register<ICommandHandler<SaveSystemProjectItemContentCommand, Resource<Empty>>>(() => new SaveSystemProjectItemContentCommandHandler(
          Locator.Current.GetService<IProjectItemContentRepository>()!));

        service.Register<ICommandHandler<SyncSolutionCommand, Resource<ProjectSolutionModel>>>(() => new SyncSolutionCommandHandler(
          Locator.Current.GetService<ISolutionRepository>()!));

        service.Register<ICommandHandler<CreateSolutionCommand, Resource<ProjectSolutionModel>>>(() => new CreateSolutionCommandHandler(
          Locator.Current.GetService<ISolutionRepository>()!));

        service
            .Register<ICommandHandler<RenameProjectFileItemCommand, Resource<ExplorerItem>>>(() => new RenameProjectFileItemCommandHandler(
     Locator.Current.GetService<IExplorerItemRepository>()!
    ));

        service
            .Register<ICommandHandler<DeleteProjectFileItemCommand, Resource<ExplorerItem>>>(() => new DeleteProjectFileItemCommandHandler(
     Locator.Current.GetService<IExplorerItemRepository>()!
    ));

        service
            .Register<ICommandHandler<DeleteProjectFolderItemCommand, Resource<ExplorerItem>>>(() => new DeleteProjectFolderItemCommandHandler(
     Locator.Current.GetService<IExplorerItemRepository>()!
    ));

        service
            .Register<ICommandHandler<RenameProjectFolderItemCommand, Resource<ExplorerItem>>>(() => new RenameProjectFolderItemCommandHandler(
     Locator.Current.GetService<IExplorerItemRepository>()!
    ));
        service
            .Register<ICommandHandler<CreateItemCommand, Resource<Empty>>>(() =>
            new CreateItemCommandHandler(
     Locator.Current.GetService<IProjectItemContentRepository>()!
    ));

        return app;
    }

    public static AppBuilder AddRepositories(this AppBuilder app)
    {
        var service = Locator.CurrentMutable;

        service
            .Register<IExplorerItemRepository>(() =>
            new ExplorerItemRepositoryImpl());

        service
            .Register<IProjectItemContentRepository>(() =>
            new ProjectItemContentRepositoryImpl());

        service
            .Register<ISolutionRepository>(() =>
            new SolutionRepositoryImpl());

        service
            .Register<IAppTemplateRepository>(() =>
            new AppTemplateRepositoryImpl());

        return app;
    }

    /*    public static void AddApplication(this IMutableDependencyResolver services, IApplication app)
        {
            services.RegisterConstant<IApplication>(app);
        }*/

    /*    public static void AddApplicationInfo(this IMutableDependencyResolver services)
        {
            services.RegisterLazySingleton<IApplicationInfo>(() =>
            {
                return new ApplicationInfo(Assembly.GetExecutingAssembly());
            });
        }*/

    private static IConfiguration BuildConfiguration() =>
       new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();


    public static AppBuilder AddConfiguration(this AppBuilder app)
    {
        var configuration = BuildConfiguration();

        var service = Locator.CurrentMutable;

        var config = new LanguagesConfiguration();
        configuration.GetSection("Languages").Bind(config);
        service.RegisterConstant(config);

        return app;
    }

    public static AppBuilder AddServices(this AppBuilder app)
    {
        var service = Locator.CurrentMutable;
        service.RegisterLazySingleton<ILanguageManager>(() => new LanguageManager(
           Locator.Current.GetService<LanguagesConfiguration>()
       ));

        return app;
    }

    public static AppBuilder AddMediator(this AppBuilder app)
    {
        var service = Locator.CurrentMutable;

        service.RegisterLazySingleton<ICommandDispatcher>(() => new CommandDispatcher(Locator.Current));
        service.RegisterLazySingleton<IQueryDispatcher>(() => new QueryDispatcher(Locator.Current));

        return app;
    }

    /*    public static IMutableDependencyResolver AddServices(this IMutableDependencyResolver service)
        {
            service.RegisterLazySingleton<IFileDialog>(() =>
            {
                return new FileDialog(Locator.Current.GetService<MainWindow>());
            });

            service.RegisterLazySingleton<INotificationMessageManagerService>(() =>
            {
                return new NotificationMessageManagerService();
            });
            return service;
        }*/

    public static IMutableDependencyResolver CreateFolderStructure(this IMutableDependencyResolver service)
    {
        if (!Directory.Exists(Constants.AppFolder))
        {
            Directory.CreateDirectory(Constants.AppFolder);
        }

        if (!Directory.Exists(Constants.AppCacheFolder))
        {
            Directory.CreateDirectory(Constants.AppCacheFolder);
        }

        if (!Directory.Exists(Constants.AppHistoryFolder))
        {
            Directory.CreateDirectory(Constants.AppHistoryFolder);
        }

        if (!Directory.Exists(Constants.AppUnclosedItemsFolder))
        {
            Directory.CreateDirectory(Constants.AppUnclosedItemsFolder);
        }

        if (!Directory.Exists(Constants.AppSettingsFolder))
        {
            Directory.CreateDirectory(Constants.AppSettingsFolder);
        }

        if (!Directory.Exists(Constants.AppUISettings))
        {
            Directory.CreateDirectory(Constants.AppUISettings);
        }

        if (!Directory.Exists(Constants.AppTemplatesFolder))
        {
            Directory.CreateDirectory(Constants.AppTemplatesFolder);
        }

        if (!Directory.Exists(Constants.AppItemsTemplateFolder))
        {
            Directory.CreateDirectory(Constants.AppItemsTemplateFolder);
        }

        return service;
    }
}
