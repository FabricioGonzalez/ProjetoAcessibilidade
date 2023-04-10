using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Extensions.Options;
using ProjectWinUI.Src.App.Contracts;
using ProjectWinUI.Src.Helpers;
using ProjectWinUI.Src.Settings.Contracts;
using ProjectWinUI.Src.Settings.Models;

namespace ProjectWinUI.Src.Settings.Services;

public class LocalSettingsService : ILocalSettingsService
{
    private const string _defaultApplicationDataFolder = "AppWinui/ApplicationData";
    private const string _defaultLocalSettingsFile = "LocalSettings.json";
    private readonly string _applicationDataFolder;

    private readonly IFileService _fileService;

    private readonly string _localApplicationData =
        Environment.GetFolderPath(folder: Environment.SpecialFolder.LocalApplicationData);

    private readonly string _localsettingsFile;
    private readonly LocalSettingsOptions _options;

    private bool _isInitialized;

    private IDictionary<string, object> _settings;

    public LocalSettingsService(
        IFileService fileService
        , IOptions<LocalSettingsOptions> options
    )
    {
        _fileService = fileService;
        _options = options.Value;

        _applicationDataFolder = Path.Combine(path1: _localApplicationData
            , path2: _options.ApplicationDataFolder ?? _defaultApplicationDataFolder);
        _localsettingsFile = _options.LocalSettingsFile ?? _defaultLocalSettingsFile;

        _settings = new Dictionary<string, object>();
    }

    public async Task<T?> ReadSettingAsync<T>(
        string key
    )
    {
        if (RuntimeHelper.IsMSIX)
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key: key, value: out var obj))
            {
                return await Json.ToObjectAsync<T>(value: (string)obj);
            }
        }
        else
        {
            await InitializeAsync();

            if (_settings != null && _settings.TryGetValue(key: key, value: out var obj))
            {
                return await Json.ToObjectAsync<T>(value: (string)obj);
            }
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(
        string key
        , T value
    )
    {
        if (RuntimeHelper.IsMSIX)
        {
            ApplicationData.Current.LocalSettings.Values[key: key] = await Json.StringifyAsync(value: value);
        }
        else
        {
            await InitializeAsync();

            _settings[key: key] = await Json.StringifyAsync(value: value);

            await Task.Run(action: () => _fileService.Save(folderPath: _applicationDataFolder
                , fileName: _localsettingsFile, content: _settings));
        }
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _settings = await Task.Run(function: () =>
                _fileService.Read<IDictionary<string, object>>(folderPath: _applicationDataFolder
                    , fileName: _localsettingsFile)) ?? new Dictionary<string, object>();

            _isInitialized = true;
        }
    }
}