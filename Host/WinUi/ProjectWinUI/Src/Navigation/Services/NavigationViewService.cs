﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using ProjectWinUI.Src.Helpers;
using ProjectWinUI.Src.Navigation.Contracts;

namespace ProjectWinUI.Src.Navigation.Services;

public class NavigationViewService : INavigationViewService
{
    private readonly INavigationService _navigationService;

    private readonly IPageService _pageService;

    private NavigationView? _navigationView;

    public NavigationViewService(
        INavigationService navigationService
        , IPageService pageService
    )
    {
        _navigationService = navigationService;
        _pageService = pageService;
    }

    public IList<object>? MenuItems => _navigationView?.MenuItems;

    public object? SettingsItem => _navigationView?.SettingsItem;

    [MemberNotNull(member: nameof(_navigationView))]
    public void Initialize(
        NavigationView navigationView
    )
    {
        _navigationView = navigationView;
        _navigationView.BackRequested += OnBackRequested;
        _navigationView.ItemInvoked += OnItemInvoked;
    }

    public void UnregisterEvents()
    {
        if (_navigationView != null)
        {
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
        }
    }

    public NavigationViewItem? GetSelectedItem(
        Type pageType
    )
    {
        if (_navigationView != null)
        {
            return GetSelectedItem(menuItems: _navigationView.MenuItems, pageType: pageType) ??
                   GetSelectedItem(menuItems: _navigationView.FooterMenuItems, pageType: pageType);
        }

        return null;
    }

    private void OnBackRequested(
        NavigationView sender
        , NavigationViewBackRequestedEventArgs args
    ) => _navigationService.GoBack();

    private void OnItemInvoked(
        NavigationView sender
        , NavigationViewItemInvokedEventArgs args
    )
    {
        if (args.IsSettingsInvoked)
        {
            /* _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);*/
        }
        else
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(dp: NavigationHelper.NavigateToProperty) is string pageKey)
            {
                if (selectedItem?.GetValue(dp: NavigationHelper.NavigateToParameterProperty) is object parameter)
                {
                    _navigationService.NavigateTo(pageKey: pageKey, parameter: parameter);
                }

                _navigationService.NavigateTo(pageKey: pageKey);
            }
        }
    }

    private NavigationViewItem? GetSelectedItem(
        IEnumerable<object> menuItems
        , Type pageType
    )
    {
        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (IsMenuItemForPageType(menuItem: item, sourcePageType: pageType))
            {
                return item;
            }

            var selectedChild = GetSelectedItem(menuItems: item.MenuItems, pageType: pageType);
            if (selectedChild != null)
            {
                return selectedChild;
            }
        }

        return null;
    }

    private bool IsMenuItemForPageType(
        NavigationViewItem menuItem
        , Type sourcePageType
    )
    {
        if (menuItem.GetValue(dp: NavigationHelper.NavigateToProperty) is string pageKey)
        {
            return _pageService.GetPageType(key: pageKey) == sourcePageType;
        }

        return false;
    }
}