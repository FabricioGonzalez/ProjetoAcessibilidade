using System.Reflection;
using Avalonia;
using Common;
using Microsoft.Extensions.Configuration;
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Common.Services;
using ProjectItemReader.InternalAppFiles;
using ProjectItemReader.XmlFile;
using ProjetoAcessibilidade.Domain.App.Contracts;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Implementations;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Contracts;
using Splat;

namespace AppDI;

public static class AvaloniaDInjector
{
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
    /*private static AppBuilder AddQueryHandlers(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;

        service.Register(factory: () => new GetProjectItemsQueryHandler(
            repository: Locator.Current.GetService<IExplorerItemRepository>()!
        ));
        service.Register<IHandler<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>>(factory: () =>
            new ReadSolutionProjectQueryHandler(
                solutionRepository: Locator.Current.GetService<ISolutionRepository>()!
            ));

        service.Register<IHandler<GetAllUfQuery, IList<UFModel>>>(factory: () => new GetAllUfQueryHandler());

        service.Register<IHandler<GetProjectItemContentQuery, Resource<AppItemModel>>>(factory: () =>
            new GetProjectItemContentQueryHandler(
                contentRepository: Locator.Current
                    .GetService<IProjectItemContentRepository>()!));
        service.Register<IHandler<GetSystemProjectItemContentQuery, Resource<AppItemModel>>>(factory: () =>
            new GetSystemProjectItemContentQueryHandler(
                contentRepository: Locator.Current
                    .GetService<IProjectItemContentRepository>()!));

        service.Register<IHandler<GetAllTemplatesQuery, Resource<List<ItemModel>>>>(factory: () =>
            new GetAllTemplatesQueryHandler(
                repository: Locator.Current.GetService<IAppTemplateRepository>()!));

        service.Register<IHandler<GetProjectItemsQuery, Resource<List<ExplorerItem>>>>(factory: () =>
            new GetProjectItemsQueryHandler(
                repository: Locator.Current.GetService<IExplorerItemRepository>()!));

        return app;
    }

    private static AppBuilder AddCommandHandlers(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;
        var resolver = Locator.Current;

        service.Register<IHandler<SaveProjectItemContentCommand, Resource<Empty>>>(factory: () =>
            new SaveProjectItemContentCommandHandler(
                content: resolver.GetService<IProjectItemContentRepository>()!));

        service.Register<IHandler<SaveSystemProjectItemContentCommand, Resource<Empty>>>(factory: () =>
            new SaveSystemProjectItemContentCommandHandler(
                content: resolver.GetService<IProjectItemContentRepository>()!));

        service.Register<IHandler<SyncSolutionCommand, Resource<ProjectSolutionModel>>>(factory: () =>
            new SyncSolutionCommandHandler(
                solutionRepository: resolver.GetService<ISolutionRepository>()!));

        service.Register<IHandler<CreateSolutionCommand, Resource<ProjectSolutionModel>>>(factory: () =>
            new CreateSolutionCommandHandler(
                solutionRepository: resolver.GetService<ISolutionRepository>()!));

        service.Register<IHandler<RenameProjectFileItemCommand, Resource<ExplorerItem>>>(factory: () =>
            new RenameProjectFileItemCommandHandler(
                repository: resolver.GetService<IExplorerItemRepository>()!
            ));

        service.Register<IHandler<DeleteProjectFileItemCommand, Resource<Empty>>>(factory: () =>
            new DeleteProjectFileItemCommandHandler(
                repository: resolver.GetService<IExplorerItemRepository>()!
            ));

        service.Register<IHandler<DeleteProjectFolderItemCommand, Resource<ExplorerItem>>>(factory: () =>
            new DeleteProjectFolderItemCommandHandler(
                repository: resolver.GetService<IExplorerItemRepository>()!
            ));

        service.Register<IHandler<RenameProjectFolderItemCommand, Resource<ExplorerItem>>>(factory: () =>
            new RenameProjectFolderItemCommandHandler(
                repository: resolver.GetService<IExplorerItemRepository>()!
            ));
        service.Register<IHandler<CreateItemCommand, Resource<Empty>>>(factory: () =>
            new CreateItemCommandHandler(
                content: resolver.GetService<IProjectItemContentRepository>()!
            ));
        service.Register<IHandler<CreateSolutionItemFolderCommand, Empty>>(factory: () =>
            new CreateSolutionItemFolderCommandHandler(
                repository: resolver.GetService<IExplorerItemRepository>()!
            ));

        return app;
    }*/

    private static AppBuilder AddRepositories(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;

        service.Register<IExplorerItemRepository>(factory: () =>
            new ExplorerItemRepositoryImpl());

        service.Register<IProjectItemContentRepository>(factory: () =>
            new ProjectItemContentRepositoryImpl());

        service.Register<ISolutionRepository>(factory: () =>
            new SolutionRepositoryImpl());

        service.Register<IAppTemplateRepository>(factory: () =>
            new AppTemplateRepositoryImpl());

        return app;
    }

    public static AppBuilder AddMediator(
        this AppBuilder app,
        params Type[] markers
    )
    {
        var handlerInfo = new Dictionary<Type, Type>();
        var notificationHandlerInfo = new Dictionary<Type, Type>();

        /*foreach (var marker in markers)
        {*/
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                     .Where(predicate: x => x.FullName.Contains(value: Constants.SolutionName)))
        {
            /*var assembly = Assembly.Load(assemblyName);*/

            var requests = GetClassesImplementingInterface(assembly: assembly, typeToMatch: typeof(IRequest<>));
            var notifications =
                GetClassesImplementingInterface(assembly: assembly, typeToMatch: typeof(INotification));
            var handlers = GetClassesImplementingInterface(assembly: assembly, typeToMatch: typeof(IHandler<,>));

            requests.ForEach(action: x =>
            {
                handlerInfo[key: x] = handlers.SingleOrDefault(predicate: xx =>
                    x == xx.GetInterface(name: "IHandler`2")!.GetGenericArguments()[0])!;
            });

            notifications.ForEach(action: x =>
            {
                notificationHandlerInfo[key: x] = handlers.SingleOrDefault(predicate: xx =>
                    x == xx.GetInterface(name: "IHandler`2")!.GetGenericArguments()[0])!;
            });

            handlers.ForEach(action: handler =>
            {
                Locator.CurrentMutable.Register(factory: () =>
                    {
                        /*var constructors = handler.GetConstructors();
                        var firstConstrutor = constructors.FirstOrDefault(); //assume we will have only one constructor
                        var parameters = new List<object>();

                        foreach (var param in firstConstrutor.GetParameters())
                        {
                            var service = Locator.Current.GetService(param.ParameterType);//get instance of the class
                            parameters.Add(service);
                        }*/

                        var instance = Activator.CreateInstance(type: handler);

                        return instance;
                    },
                    serviceType: handler);
            });
        }
        /*}*/

        Locator.CurrentMutable.RegisterLazySingleton<IMediator>(valueFactory: () =>
            new Mediator(serviceResolver: type => Locator.Current.GetService(serviceType: type),
                handlerDetails: handlerInfo,
                notificationsDetails: notificationHandlerInfo));

        return app;
    }


    private static List<Type> GetClassesImplementingInterface(Assembly assembly, Type typeToMatch) =>
        assembly.ExportedTypes.Where(
            predicate: type =>
            {
                var genericInterfaceTypes = type.GetInterfaces().Where(
                    predicate: x => x.IsGenericType).ToList();
                var implementRequestType =
                    genericInterfaceTypes.Any(predicate: x => x.GetGenericTypeDefinition() == typeToMatch);

                return !type.IsInterface && !type.IsAbstract && implementRequestType;
            }).ToList();

    private static AppBuilder AddServices(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;
        service.RegisterLazySingleton<ILanguageManager>(valueFactory: () => new LanguageManager(
            configuration: Locator.Current.GetService<LanguagesConfiguration>()
        ));

        return app;
    }

    private static AppBuilder AddMediator(
        this AppBuilder app
    )
    {
        var service = Locator.CurrentMutable;
        /*DependencyResolverMixins.RegisterLazySingleton<ICommandDispatcher>(resolver: service, valueFactory: () =>
            new CommandDispatcher(serviceProvider: Locator.Current));*/
        /*service.RegisterLazySingleton<ISender>(valueFactory: () =>
            new QueryDispatcher(serviceProvider: Locator.Current));*/

        return app;
    }

    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .AddJsonFile(path: "appsettings.json")
            .Build();


    private static AppBuilder AddConfiguration(
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

    public static AppBuilder StartContainer(this AppBuilder app) =>
        app
            .AddConfiguration()
            .AddServices()
            .AddRepositories();
}