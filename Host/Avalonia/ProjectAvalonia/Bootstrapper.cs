using System.Collections.Generic;
using System.IO;
using Avalonia;
using Common;
using Core.Entities.App;
using Core.Entities.Solution;
using Core.Entities.Solution.Explorer;
using Core.Entities.Solution.ItemsGroup;
using Core.Entities.Solution.Project.AppItem;
using Microsoft.Extensions.Configuration;
using Project.Domain.App.Contracts;
using Project.Domain.App.Models;
using Project.Domain.App.Queries.Templates;
using Project.Domain.App.Queries.UF;
using Project.Domain.Contracts;
using Project.Domain.Implementations;
using Project.Domain.Project.Commands.FolderItems;
using Project.Domain.Project.Commands.ProjectItems;
using Project.Domain.Project.Commands.SystemItems;
using Project.Domain.Project.Contracts;
using Project.Domain.Project.Queries.ProjectItems;
using Project.Domain.Project.Queries.SystemItems;
using Project.Domain.Solution.Commands.SolutionItem;
using Project.Domain.Solution.Contracts;
using Project.Domain.Solution.Queries;
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Common.Services;
using ProjectItemReader.InternalAppFiles;
using ProjectItemReader.XmlFile;
using Splat;

namespace ProjectAvalonia;

public static class Bootstrapper
{
    public static AppBuilder AddQueryHandlers(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;

        service
            .Register(factory: () => new GetProjectItemsQueryHandler(
                repository: Locator.Current.GetService<IExplorerItemRepository>()!
            ));
        service
            .Register<IQueryHandler<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>>(factory: () =>
                new ReadSolutionProjectQueryHandler(
                    solutionRepository: Locator.Current.GetService<ISolutionRepository>()!
                ));

        service
            .Register<IQueryHandler<GetAllUfQuery, IList<UFModel>>>(factory: () => new GetAllUfQueryHandler());

        service
            .Register<IQueryHandler<GetProjectItemContentQuery, Resource<AppItemModel>>>(factory: () =>
                new GetProjectItemContentQueryHandler(
                    contentRepository: Locator.Current.GetService<IProjectItemContentRepository>()!));
        service
            .Register<IQueryHandler<GetSystemProjectItemContentQuery, Resource<AppItemModel>>>(factory: () =>
                new GetSystemProjectItemContentQueryHandler(
                    contentRepository: Locator.Current.GetService<IProjectItemContentRepository>()!));

        service
            .Register<IQueryHandler<GetAllTemplatesQuery, Resource<List<ItemModel>>>>(factory: () =>
                new GetAllTemplatesQueryHandler(
                    repository: Locator.Current.GetService<IAppTemplateRepository>()!));

        service
            .Register<IQueryHandler<GetProjectItemsQuery, Resource<List<ExplorerItem>>>>(factory: () =>
                new GetProjectItemsQueryHandler(
                    repository: Locator.Current.GetService<IExplorerItemRepository>()!));

        return app;
    }

    public static AppBuilder AddCommandHandlers(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;
        var resolver = Locator.Current;

        service.Register<ICommandHandler<SaveProjectItemContentCommand, Resource<Empty>>>(factory: () =>
            new SaveProjectItemContentCommandHandler(
                content: resolver.GetService<IProjectItemContentRepository>()!));

        service.Register<ICommandHandler<SaveSystemProjectItemContentCommand, Resource<Empty>>>(factory: () =>
            new SaveSystemProjectItemContentCommandHandler(
                content: resolver.GetService<IProjectItemContentRepository>()!));

        service.Register<ICommandHandler<SyncSolutionCommand, Resource<ProjectSolutionModel>>>(factory: () =>
            new SyncSolutionCommandHandler(
                solutionRepository: resolver.GetService<ISolutionRepository>()!));

        service.Register<ICommandHandler<CreateSolutionCommand, Resource<ProjectSolutionModel>>>(factory: () =>
            new CreateSolutionCommandHandler(
                solutionRepository: resolver.GetService<ISolutionRepository>()!));

        service
            .Register<ICommandHandler<RenameProjectFileItemCommand, Resource<ExplorerItem>>>(factory: () =>
                new RenameProjectFileItemCommandHandler(
                    repository: resolver.GetService<IExplorerItemRepository>()!
                ));

        service
            .Register<ICommandHandler<DeleteProjectFileItemCommand, Resource<Empty>>>(factory: () =>
                new DeleteProjectFileItemCommandHandler(
                    repository: resolver.GetService<IExplorerItemRepository>()!
                ));

        service
            .Register<ICommandHandler<DeleteProjectFolderItemCommand, Resource<ExplorerItem>>>(factory: () =>
                new DeleteProjectFolderItemCommandHandler(
                    repository: resolver.GetService<IExplorerItemRepository>()!
                ));

        service
            .Register<ICommandHandler<RenameProjectFolderItemCommand, Resource<ExplorerItem>>>(factory: () =>
                new RenameProjectFolderItemCommandHandler(
                    repository: resolver.GetService<IExplorerItemRepository>()!
                ));
        service
            .Register<ICommandHandler<CreateItemCommand, Resource<Empty>>>(factory: () =>
                new CreateItemCommandHandler(
                    content: resolver.GetService<IProjectItemContentRepository>()!
                ));
        service
            .Register<ICommandHandler<CreateSolutionItemFolderCommand, Empty>>(factory: () =>
                new CreateSolutionItemFolderCommandHandler(
                    repository: resolver.GetService<IExplorerItemRepository>()!
                ));

        return app;
    }

    public static AppBuilder AddRepositories(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;

        service
            .Register<IExplorerItemRepository>(factory: () =>
                new ExplorerItemRepositoryImpl());

        service
            .Register<IProjectItemContentRepository>(factory: () =>
                new ProjectItemContentRepositoryImpl());

        service
            .Register<ISolutionRepository>(factory: () =>
                new SolutionRepositoryImpl());

        service
            .Register<IAppTemplateRepository>(factory: () =>
                new AppTemplateRepositoryImpl());

        return app;
    }

    public static AppBuilder AddStateStores(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;
        var resolver = Locator.Current;

        /*service.RegisterConstant(
            value: new TemplateItemsStore(queryDispatcher: resolver.GetService<IQueryDispatcher>()));

        service.RegisterConstant(
            value: new SolutionStore(queryDispatcher: resolver.GetService<IQueryDispatcher>()));

        service.RegisterConstant(
            value: new AddressesStore(queryDispatcher: resolver.GetService<IQueryDispatcher>()));

        service.RegisterConstant(
            value: new EditingItemsStore(queryDispatcher: resolver.GetService<IQueryDispatcher>()));

        service.RegisterConstant(
            value: new ExplorerItemsStore(solutionStore: resolver.GetService<SolutionStore>(),
                commandDispatcher: resolver.GetService<ICommandDispatcher>()));*/

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
            .AddJsonFile(path: "appsettings.json")
            .Build();


    public static AppBuilder AddConfiguration(
        this AppBuilder app
    )
    {
        var configuration = BuildConfiguration();

        var service = Locator.CurrentMutable;

        var config = new LanguagesConfiguration();
        configuration.GetSection(key: "Languages").Bind(instance: config);
        service.RegisterConstant(value: config);

        return app;
    }

    public static AppBuilder AddServices(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;
        service.RegisterLazySingleton<ILanguageManager>(valueFactory: () => new LanguageManager(
            configuration: Locator.Current.GetService<LanguagesConfiguration>()
        ));

        return app;
    }

    public static AppBuilder AddMediator(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;

        service.RegisterLazySingleton<ICommandDispatcher>(valueFactory: () =>
            new CommandDispatcher(serviceProvider: Locator.Current));
        service.RegisterLazySingleton<IQueryDispatcher>(valueFactory: () =>
            new QueryDispatcher(serviceProvider: Locator.Current));

        return app;
    }

    public static IMutableDependencyResolver CreateFolderStructure(
        this IMutableDependencyResolver service
    )
    {
        if (!Directory.Exists(path: Constants.AppFolder))
        {
            Directory.CreateDirectory(path: Constants.AppFolder);
        }

        if (!Directory.Exists(path: Constants.AppCacheFolder))
        {
            Directory.CreateDirectory(path: Constants.AppCacheFolder);
        }

        if (!Directory.Exists(path: Constants.AppHistoryFolder))
        {
            Directory.CreateDirectory(path: Constants.AppHistoryFolder);
        }

        if (!Directory.Exists(path: Constants.AppUnclosedItemsFolder))
        {
            Directory.CreateDirectory(path: Constants.AppUnclosedItemsFolder);
        }

        if (!Directory.Exists(path: Constants.AppSettingsFolder))
        {
            Directory.CreateDirectory(path: Constants.AppSettingsFolder);
        }

        if (!Directory.Exists(path: Constants.AppUISettings))
        {
            Directory.CreateDirectory(path: Constants.AppUISettings);
        }

        if (!Directory.Exists(path: Constants.AppTemplatesFolder))
        {
            Directory.CreateDirectory(path: Constants.AppTemplatesFolder);
        }

        if (!Directory.Exists(path: Constants.AppItemsTemplateFolder))
        {
            Directory.CreateDirectory(path: Constants.AppItemsTemplateFolder);
        }

        return service;
    }
}