using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using LanguageExt.Common;

namespace ProjectAvalonia.Common.Helpers;

public static class FileDialogHelper
{
    private static IStorageProvider GetProvider()
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime app)
        {
            return TopLevel.GetTopLevel(app.MainView).StorageProvider;
        }

        return ((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime).MainWindow
            .StorageProvider;
    }

    /*public static async Task<string?> ShowOpenFileDialogAsync(
        string title
    )
    {
        var ofd = CreateOpenFileDialog(title: title);
        return await GetDialogResultAsync(ofd: ofd);
    }

    public static async Task<string?> ShowOpenFileDialogAsync(
        string title
        , string[] filterExtTypes
        , string? initialFileName = null
        , string? directory = null
    )
    {
        var ofd = CreateOpenFileDialog(title: title, directory: directory);
        ofd.InitialFileName = initialFileName;
        ofd.Filters = GenerateFilters(filterExtTypes: filterExtTypes);
        return await GetDialogResultAsync(ofd: ofd);
    }

    public static async Task<string?> ShowSaveFileDialogAsync(
        string title
        , string[] filterExtTypes
        , string? initialFileName = null
        , string? directory = null
    )
    {
        var sfd = CreateSaveFileDialog(title: title, filterExtTypes: filterExtTypes, directory: directory);
        sfd.InitialFileName = initialFileName;
        sfd.Filters = GenerateFilters(filterExtTypes: filterExtTypes);
        return await GetDialogResultAsync(sfd: sfd);
    }

    public static async Task<string?> ShowOpenFolderDialogAsync(
        string title
        , string? directory = null
    )
    {
        var sfd = CreateOpenFolderDialog(title: title, directory: directory);

        return await GetDialogResultAsync(ofd: sfd);
    }

    private static SaveFileDialog CreateSaveFileDialog(
        string title
        , IEnumerable<string> filterExtTypes
        , string? directory = null
    )
    {
        var sfd = new SaveFileDialog
        {
            DefaultExtension = filterExtTypes.FirstOrDefault(), Title = title, Directory = directory
        };

        return sfd;
    }

    private static async Task<string?> GetDialogResultAsync(
        OpenFileDialog ofd
    )
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime &&
            lifetime.MainWindow is not null)
        {
            var selected = await ofd.ShowAsync(parent: lifetime.MainWindow);

            return selected?.FirstOrDefault();
        }

        return null;
    }

    private static async Task<string?> GetDialogResultAsync(
        OpenFolderDialog ofd
    )
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime &&
            lifetime.MainWindow is not null)
        {
            var selected = await ofd.ShowAsync(parent: lifetime.MainWindow);

            return selected;
        }

        return null;
    }

    private static async Task<string?> GetDialogResultAsync(
        SaveFileDialog sfd
    )
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime &&
            lifetime.MainWindow is not null)
        {
            return await sfd.ShowAsync(parent: lifetime.MainWindow);
        }

        return null;
    }

    private static OpenFileDialog CreateOpenFileDialog(
        string title
        , string? directory = null
    )
    {
        var ofd = new OpenFileDialog
        {
            AllowMultiple = false, Title = title
        };

        if (directory is null)
        {
            SetDefaultDirectory(sfd: ofd);
        }
        else
        {
            ofd.Directory = directory;
        }

        return ofd;
    }

    private static OpenFolderDialog CreateOpenFolderDialog(
        string title
        , string? directory = null
    )
    {
        var ofd = new OpenFolderDialog
        {
            Title = title
        };

        if (directory is null)
        {
            SetDefaultDirectory(sfd: ofd);
        }
        else
        {
            ofd.Directory = directory;
        }

        return ofd;
    }

    private static List<FileDialogFilter> GenerateFilters(
        string[] filterExtTypes
    )
    {
        var filters = new List<FileDialogFilter>();

        var generatedFilters =
            filterExtTypes
                .Where(predicate: x => x != "*")
                .Select(selector: ext =>
                    new FileDialogFilter
                    {
                        Name = $"{ext.ToUpper()} files", Extensions = new List<string> { ext }
                    });

        filters.AddRange(collection: generatedFilters);

        if (filterExtTypes.Contains(value: "*"))
        {
            filters.Add(item: new FileDialogFilter
            {
                Name = "All files", Extensions = new List<string> { "*" }
            });
        }

        return filters;
    }

    private static void SetDefaultDirectory(
        FileSystemDialog sfd
    )
    {
        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Linux))
        {
            sfd.Directory = Path.Combine(path1: "/media", path2: Environment.UserName);
        }

        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.OSX))
        {
            sfd.Directory = Environment.GetFolderPath(folder: Environment.SpecialFolder.Personal);
        }
    }*/
    public static async Task<Result<IStorageFolder>> GetFolderAsync()
    {
        var folders = await GetProvider().OpenFolderPickerAsync(new FolderPickerOpenOptions { Title = "Abrir pasta" });

        return folders.Any()
            ? new Result<IStorageFolder>(folders.First())
            : new Result<IStorageFolder>(new IOException("No folder was Selected"));
    }

    public static async Task<Result<IStorageFile>> GetFileAsync()
    {
        var files = await GetProvider()
            .OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Abrir file", FileTypeFilter = new List<FilePickerFileType>
                {
                    new("Arquivo de projeto") { Patterns = new[] { "*.prja" } }
                }
            });

        return files.Any()
            ? new Result<IStorageFile>(files.First())
            : new Result<IStorageFile>(new IOException("No file was Selected"));
    }
}