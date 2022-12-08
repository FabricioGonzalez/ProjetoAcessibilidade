﻿using Splat;

using System.Collections.Generic;

using Common;

using ProjectAvalonia.Views;
using ProjectAvalonia.Project.Components.ProjectExplorer;
using ProjectAvalonia.Services;

using ProjectItemReader.InternalAppFiles;
using ProjectItemReader.XmlFile;

using ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;
using System.IO;

using Project.Core.ViewModels.TemplateEditing;
using Project.Core.ViewModels.Project;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.FileTemplate;
using AppUsecases.Usecases;
using AppUsecases.Contracts.Repositories;
using AppUsecases.Project.Entities.Project;
using AppUsecases.App.Usecases;
using AppUsecases.Editing.Entities;
using UIStatesStore.Project.Models;
using UIStatesStore.Contracts;
using UIStatesStore.App.Models;
using UIStatesStore.Project.Observable;

using UIStatesStore.App.Observable;
using UIStatesStore.Solution.Observables;
using AppViewModels.Main;
using AppViewModels.Contracts;
using Project.Application.Project.Contracts;

namespace ProjectAvalonia;
public static class Bootstrapper
{
    public static IMutableDependencyResolver AddViewModel(this IMutableDependencyResolver service)
    {
        /*service.RegisterLazySingleton(() => new MainWindowViewModel());*/
        service.RegisterLazySingleton(() => new MainViewModel());

        service.RegisterLazySingleton(() => new TemplateEditingViewModel());

        service.RegisterLazySingleton(() => new TemplateRulesViewModel());

        service.RegisterLazySingleton(() => new AddItemViewModel());

        /*service.RegisterLazySingleton(() => new ProjectViewModel());*/
        service.RegisterLazySingleton(() => new AppViewModels.Project.ProjectViewModel());

        /*service.Register(() => new ExplorerComponentViewModel());*/
        service.Register(() => new AppViewModels.Project.ProjectExplorerViewModel());
        service.Register(() => new ItemEditingViewModel());

        return service;
    }
    public static IMutableDependencyResolver AddViewComponents(this IMutableDependencyResolver service)
    {
        service.Register(() => new ExplorerComponent());

        service.RegisterLazySingleton(() => new MainWindow());

        service.Register(() => new AddItemWindow());

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

        service.Register<ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel>>(() => new CreateProjectSolutionUsecase(
            Locator.Current.GetService<IWriteContract<ProjectSolutionModel>>()));  
        
        service.Register(() => new GetUFList());

        return service;
    }
    public static IMutableDependencyResolver AddRepositories(this IMutableDependencyResolver service)
    {
        service
            .Register<IWriteContract<ProjectSolutionModel>>(() =>
            new WriteUserSolutionRepository());
               
        service
            .Register <IExplorerItemRepository > (() =>
            new ExplorerItemRepositoryImpl());

        service
            .Register<IWriteContract<AppItemModel>>(() =>
            new WriteTemplateContentRepository());

        service
    .Register<IReadContract<AppItemModel>>(() =>
    new ReadTemplateContentRepository());

        service
            .Register<IReadContract<ProjectSolutionModel>>(() =>
            new ReadUserSolutionRepository());

        service
            .Register<IReadContract<List<FileTemplate>>>(() =>
            new ReadAllRecentProjectFilesRepository());

        service
            .Register<IReadContract<Resource<List<FileTemplate>>>>(() =>
            new ReadAppTemplatesRepository());

        service
            .Register<IReadContract<List<ExplorerItem>>>(() =>
            new ReadProjectFilesRepository());

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

    public static IMutableDependencyResolver AddUIStates(this IMutableDependencyResolver service)
    {
        service.RegisterLazySingleton<IAppObservable<ProjectModel>>(() => new ProjectStateObservable());
        service.RegisterLazySingleton<IAppObservable<AppErrorMessage>>(() => new AppErrorObservable());
        service.RegisterLazySingleton<IAppObservable<ProjectEditingModel>>(() => new ProjectEditingStateObservable());
        service.RegisterLazySingleton<IAppObservable<UIStatesStore.Solution.Models.ProjectSolutionModel>>(() => new SolutionObservable());

        return service;
    }

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
