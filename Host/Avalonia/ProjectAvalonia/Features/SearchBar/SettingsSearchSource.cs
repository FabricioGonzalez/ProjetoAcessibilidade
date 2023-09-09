using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using DynamicData;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Features.SearchBar.Patterns;
using ProjectAvalonia.Features.SearchBar.SearchItems;
using ProjectAvalonia.Features.SearchBar.Sources;
using ProjectAvalonia.Features.Settings.ViewModels;
using ProjectAvalonia.ViewModels.SearchBar.Settings;

namespace ProjectAvalonia.Features.SearchBar;

public class SettingsSearchSource : ISearchSource
{
    private readonly SettingsPageViewModel _settingsPage;

    public SettingsSearchSource(
        SettingsPageViewModel settingsPage
        , IObservable<string> query
    )
    {
        _settingsPage = settingsPage;

        var filter = query.Select(SearchSource.DefaultFilter);

        Changes = GetSettingsItems()
            .ToObservable()
            .ToObservableChangeSet(x => x.Key)
            .Filter(filter);
    }

    public IObservable<IChangeSet<ISearchItem, ComposedKey>> Changes
    {
        get;
    }

    private IEnumerable<ISearchItem> GetSettingsItems() =>
        new ISearchItem[]
        {
            new NonActionableSearchItem(new Setting<GeneralSettingsTabViewModel, bool>(
                    _settingsPage.GeneralSettingsTab,
                    b => b.DarkModeEnabled), "DarkModeLabel".GetLocalized(),
                "AppearanceLabel".GetLocalized(),
                new List<string> { "ThemeKeyWords".GetLocalized().Split(",") },
                "nav_settings_regular") { IsDefault = false }
            , new NonActionableSearchItem(new Setting<GeneralSettingsTabViewModel, bool>
                (_settingsPage.GeneralSettingsTab,
                    b => b.AutoCopy),
                "Auto copy addresses",
                "Settings",
                new List<string>(),
                "nav_settings_regular") { IsDefault = false }
            , new NonActionableSearchItem(new Setting<GeneralSettingsTabViewModel, bool>
                    (_settingsPage.GeneralSettingsTab,
                        b => b.AutoPaste),
                    "Auto paste addresses",
                    "Settings",
                    new List<string>(),
                    "nav_settings_regular")
                { IsDefault = false }
            , new NonActionableSearchItem(new Setting<GeneralSettingsTabViewModel, bool>
                    (_settingsPage.GeneralSettingsTab,
                        b => b.HideOnClose),
                    "Run in background when closed",
                    "Settings",
                    new List<string> { "hide", "tray" },
                    "nav_settings_regular")
                { IsDefault = false }
            , new NonActionableSearchItem(new Setting<GeneralSettingsTabViewModel, bool>
                    (_settingsPage.GeneralSettingsTab,
                        b => b.RunOnSystemStartup),
                    "Run Wasabi when computer starts",
                    "Settings",
                    new List<string> { "startup", "boot" },
                    "nav_settings_regular")
                { IsDefault = false }
            , new NonActionableSearchItem(new Setting<AdvancedSettingsTabViewModel, bool>
                    (_settingsPage.AdvancedSettingsTab,
                        b => b.EnableGpu),
                    "Enable GPU",
                    "Settings",
                    new List<string>(),
                    "nav_settings_regular")
                { IsDefault = false }
        };
}