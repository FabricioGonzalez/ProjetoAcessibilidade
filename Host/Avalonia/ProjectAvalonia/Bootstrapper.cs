using Splat;

using Project.Core.ViewModels;
using Project.Core.Contracts;

using System.Collections.Generic;

using AppUsecases.Usecases;
using AppUsecases.Project.Entities.Project;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Editing.Entities;
using AppUsecases.App.Usecases;
using AppUsecases.Contracts.Repositories;
using AppUsecases.Project.Entities.FileTemplate;

using Common;

using ProjectAvalonia.Views;
using ProjectAvalonia.Project.Components.ProjectExplorer;
using ProjectAvalonia.Services;

using UIStatesStore.Project.Observable;
using UIStatesStore.Project.Models;
using UIStatesStore.Contracts;

using ProjectItemReader.InternalAppFiles;
using ProjectItemReader.XmlFile;
using UIStatesStore.App.Models;
using UIStatesStore.App.Observable;

namespace ProjectAvalonia;
public static class Bootstrapper
{
    public static IMutableDependencyResolver AddViewModel(this IMutableDependencyResolver service)
    {
        service.Register(() => new ExplorerComponentViewModel());

        return service;
    }
    public static IMutableDependencyResolver AddViewComponents(this IMutableDependencyResolver service)
    {
        service.Register(() => new ExplorerComponent());
        service.Register(() => new MainWindow());

        return service;
    }
    public static IMutableDependencyResolver AddUsecases(this IMutableDependencyResolver service)
    {
        service.Register<IQueryUsecase<string, List<ExplorerItem>>>(() => new GetProjectItemsUsecase(
             Locator.Current.GetService<IReadContract<List<ExplorerItem>>>()
            ));

        service.Register<IQueryUsecase<ProjectSolutionModel>>(() => new GetProjectSolutionUsecase(
            Locator.Current.GetService<IReadContract<ProjectSolutionModel>>()
            ));

        service.Register<IQueryUsecase<List<FileTemplate>>>(() => new GetProjectTemplateUsecase(
            Locator.Current.GetService<IReadContract<Resource<List<FileTemplate>>>>()
            ));

        service.Register<IQueryUsecase<string, AppItemModel>>(() => new GetProjectItemContentUsecase(
            Locator.Current.GetService<IReadContract<AppItemModel>>()
            ));

        service.Register<ICommandUsecase<ProjectSolutionModel>>(() => new CreateProjectSolutionUsecase());

        return service;
    }
    public static IMutableDependencyResolver AddRepositories(this IMutableDependencyResolver service)
    {
        service
            .Register<IWriteContract<ProjectSolutionModel>>(() =>
            new WriteUserProjectSolutionFileRepository());

        service
            .Register<IWriteContract<AppItemModel>>(() =>
            new WriteXmlFileRepository());

        service
    .Register<IReadContract<AppItemModel>>(() =>
    new ReadXmlFileRepository());

        service
            .Register<IReadContract<ProjectSolutionModel>>(() =>
            new ReadUserProjectSolutionFileRepository());

        service
            .Register<IReadContract<List<FileTemplate>>>(() =>
            new ReadAllRecentProjectFilesRepository());

        service
            .Register<IReadContract<Resource<List<FileTemplate>>>>(() =>
            new ReadAllProjectTemplateFilesRepository());

        service
            .Register<IReadContract<List<ExplorerItem>>>(() =>
            new ReadAllUserProjectTemplateFilesRepository());

        return service;
    }
    /* public static void AddApplication(this IMutableDependencyResolver services, IApplication app)
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

    /*    public static void AddAutomapper(this IMutableDependencyResolver services)
        {
            services.RegisterLazySingleton(() =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MappingProfile());
                });

                return config.CreateMapper();
            });
        }*/

    /*    public static void AddSettingsProvider(this IMutableDependencyResolver services)
        {
            string settingsPath = "";

            var platformInfo = AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetRuntimeInfo();

            AppSettings defaultAppSettings = null;

            if (platformInfo.OperatingSystem == OperatingSystemType.WinNT)
            {
                settingsPath = Environment.ExpandEnvironmentVariables("%APPDATA%\\SampleAvaloniaApplicationClient\\appsettings.json");
                defaultAppSettings = new AppSettings();
            }
            else if (platformInfo.OperatingSystem == OperatingSystemType.Linux)
            {
                settingsPath = Environment.ExpandEnvironmentVariables("/%HOME%/SampleAvaloniaApplicationClient/appsettings.json");
                defaultAppSettings = new AppSettings()
                {
                    DbFilename = "/%HOME%/SampleAvaloniaApplicationClient/data.db",
                    LogsFolder = "/%HOME%/SampleAvaloniaApplicationClient/Logs"
                };
            }

            var settingsProvider = new JsonSettingsProvider<AppSettings>(settingsPath, defaultAppSettings, new JsonSerializerOptions() { WriteIndented = true });

            services.RegisterLazySingleton<ISettingsProvider<AppSettings>>(() =>
                new JsonSettingsProvider<AppSettings>(settingsPath, defaultAppSettings, new JsonSerializerOptions() { WriteIndented = true }));
        }*/

    /*    public static void AddLogging(this IMutableDependencyResolver services)
        {
            services.RegisterLazySingleton(() =>
            {
                var settings = Locator.Current.GetService<ISettingsProvider<AppSettings>>();

                var logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.File(Path.Combine(Environment.ExpandEnvironmentVariables(settings.Settings.LogsFolder), "log-.txt"),
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();

                return new SerilogLoggerProvider(logger).CreateLogger(nameof(SampleAvaloniaApplicationClientApplication));
            });
        }*/
    public static IMutableDependencyResolver AddServices(this IMutableDependencyResolver service)
    {
        service.RegisterLazySingleton<IFileDialog>(() =>
        {
            return new FileDialog(Locator.Current.GetService<MainWindow>());
        });
        return service;
    }

    public static void AddUIStates(this IMutableDependencyResolver service)
    {
        service.RegisterLazySingleton<IAppObservable<ProjectModel>>(() => new ProjectStateObservable());
        service.RegisterLazySingleton<IAppObservable<AppErrorMessage>>(() => new AppErrorObservable());
    }
}
