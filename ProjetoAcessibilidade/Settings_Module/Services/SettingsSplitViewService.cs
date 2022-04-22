using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;

namespace ProjetoAcessibilidade.Settings_Module.Services
{
    public class SettingsSplitViewService
    {
        static Dictionary<Type, Type> pages = new Dictionary<Type, Type>();

        public static void RegisterPages<TView, TViewModel>()
        {
            pages.Add(typeof(TViewModel), typeof(TView));
        }

        public void SetContent(Type type)
        {
            var PageType = pages[type];

            var page = (Page)App.MainWindow.Content;
            var navigation = (NavigationView)page.Content;

            var frame = (Frame)navigation.Content;

            if (frame.Content != null)
            {
                var page2 = (Page)frame.Content;

                var grid = (Grid)page2.Content;

                var splitView = (SplitView)grid.Children[0];
                var content = Ioc.Default.GetService(PageType);

                App.EnqueueProcess(() =>
                {
                    (content as FrameworkElement).DataContext = Ioc.Default.GetService(type);

                    splitView.Content = (Page)content;
                });
            }
        }
    }
}
