using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using DynamicData;

using ProjectAvalonia.Features.SearchBar.Patterns;
using ProjectAvalonia.Features.SearchBar.SearchItems;
using ProjectAvalonia.Features.SearchBar.Sources;
using ProjectAvalonia.Features.SearchBars.ViewModels.SearchBar.Sources;
using ProjectAvalonia.Features.Settings.ViewModels;
using ProjectAvalonia.ViewModels.SearchBar.Settings;

namespace ProjectAvalonia.Features.SearchBar;

public class SettingsSearchSource : ISearchSource
{
    private readonly SettingsPageViewModel _settingsPage;

    public SettingsSearchSource(SettingsPageViewModel settingsPage, IObservable<string> query)
    {
        _settingsPage = settingsPage;

        var filter = query.Select(SearchSource.DefaultFilter);

        Changes = GetSettingsItems()
            .ToObservable()
            .ToObservableChangeSet(keySelector: x => x.Key)
            .Filter(predicateChanged: filter);
    }

    public IObservable<IChangeSet<ISearchItem, ComposedKey>> Changes
    {
        get;
    }

    private IEnumerable<ISearchItem> GetSettingsItems()
    {
        return new ISearchItem[]
        {
            new NonActionableSearchItem(content: new Setting<GeneralSettingsTabViewModel, bool>(target: _settingsPage.GeneralSettingsTab,
            selector: b => b.DarkModeEnabled), name: "Dark mode",
            category: "Appearance",
            keywords: new List<string> { "Black", "White", "Theme", "Dark", "Light" },
            icon: "nav_settings_regular") { IsDefault = false },
            new NonActionableSearchItem(content: new Setting<GeneralSettingsTabViewModel, bool>(target: _settingsPage.GeneralSettingsTab, selector: b => b.AutoCopy),
                                        name: "Auto copy addresses",
                                        category: "Settings",
                                        keywords: new List<string>(),
                                        icon: "nav_settings_regular") { IsDefault = false },
            new NonActionableSearchItem(content: new Setting<GeneralSettingsTabViewModel, bool>(target: _settingsPage.GeneralSettingsTab,
                                                                                                selector: b => b.AutoPaste), name: "Auto paste addresses", category: "Settings", keywords: new List<string>(), icon: "nav_settings_regular") { IsDefault = false },
            new NonActionableSearchItem(content: new Setting<GeneralSettingsTabViewModel, bool>(target: _settingsPage.GeneralSettingsTab,
                                                                                                selector: b => b.HideOnClose), name: "Run in background when closed", category: "Settings", keywords: new List<string>() { "hide", "tray" }, icon: "nav_settings_regular") { IsDefault = false },
            new NonActionableSearchItem(content: new Setting<GeneralSettingsTabViewModel, bool>(target: _settingsPage.GeneralSettingsTab,
                                                                                                selector: b => b.RunOnSystemStartup), name: "Run Wasabi when computer starts", category: "Settings", keywords: new List<string>() { "startup", "boot" }, icon: "nav_settings_regular") { IsDefault = false },
            new NonActionableSearchItem(content: new Setting<AdvancedSettingsTabViewModel, bool>(target: _settingsPage.AdvancedSettingsTab,
                                                                                                 selector: b => b.EnableGpu), name: "Enable GPU", category: "Settings", keywords: new List<string>(), icon: "nav_settings_regular") { IsDefault = false },
        };
    }
}
