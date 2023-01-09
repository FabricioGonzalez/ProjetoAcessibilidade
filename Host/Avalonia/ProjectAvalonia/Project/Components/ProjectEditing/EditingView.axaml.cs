using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

using AppViewModels.Project;
using AppViewModels.Project.ComposableViewModels;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DynamicData.Binding;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Project.Components.ProjectEditing;
public partial class EditingView : ReactiveUserControl<ProjectEditingViewModel>
{
    public TabControl TabHost => this.FindControl<TabControl>("ProjectEditingTabHost");
    public EditingView()
    {
        ViewModel ??= Locator.Current.GetService<ProjectEditingViewModel>();

        DataContext = ViewModel;

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.WhenValueChanged(v => v.ViewModel.SelectedItem)
            .Subscribe((prop) =>
            {
                TabHost.RaiseEvent(new SelectionChangedEventArgs(
                    Avalonia.Controls.Primitives.SelectingItemsControl.SelectionChangedEvent,
                    removedItems: new List<FileProjectItemViewModel>(),
                    addedItems: new List<FileProjectItemViewModel>() { prop }));

                TabHost.SelectedItem = prop;

            })
            .DisposeWith(disposables);

            TabHost.AddHandler(Avalonia.Controls.Primitives.SelectingItemsControl.SelectionChangedEvent, (sender, args) =>
            {
                var items = args.AddedItems;

                if (items.Count > 0)
                {
                    ViewModel.SelectedItem = (AppViewModels.Project.ComposableViewModels.FileProjectItemViewModel)items[0];

                }
            });

        });

        AvaloniaXamlLoader.Load(this);
    }
}
