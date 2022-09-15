﻿using AppWinui.AppCode.AppUtils.Contracts.Services;
using AppWinui.AppCode.AppUtils.ViewModels;
using AppWinui.AppCode.AppUtils.Views;
using AppWinui.AppCode.Home.ViewModels;
using AppWinui.AppCode.Home.Views;
using AppWinui.AppCode.Project.ViewModels;
using AppWinui.AppCode.Project.Views;
using AppWinui.AppCode.TemplateEditing.ViewModels;
using AppWinui.AppCode.TemplateEditing.Views;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.AppUtils.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<MainViewModel, MainPage>();
        Configure<ListDetailsViewModel, ListDetailsPage>();
        Configure<SettingsViewModel, SettingsPage>();
        Configure<ProjectViewModel, ProjectPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}