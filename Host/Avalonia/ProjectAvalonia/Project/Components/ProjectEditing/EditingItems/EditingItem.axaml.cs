using System;
using System.Diagnostics;
using System.Reactive.Disposables;

using AppViewModels.Project;
using AppViewModels.Project.ComposableViewModels;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DynamicData.Binding;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Project.Components.ProjectEditing.EditingItems;
public partial class EditingItem : ReactiveUserControl<ProjectItemEditingViewModel>
{

    public static readonly AttachedProperty<FileProjectItemViewModel> ItemProperty =
          AvaloniaProperty.RegisterAttached<EditingItem, ReactiveUserControl<ProjectItemEditingViewModel>, FileProjectItemViewModel>(nameof(Item));

    public FileProjectItemViewModel? Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public EditingItem()
    {
        ViewModel ??= Locator.Current.GetService<ProjectItemEditingViewModel>();

        DataContext = ViewModel;

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            ViewModel.Activator.Activate();

            this.WhenPropertyChanged(prop => prop.Item)
            .Subscribe(prop =>
            {
                Debug.WriteLine(prop.Value.Path);
            })
            .DisposeWith(disposables);

        });

        AvaloniaXamlLoader.Load(this);
    }
}
