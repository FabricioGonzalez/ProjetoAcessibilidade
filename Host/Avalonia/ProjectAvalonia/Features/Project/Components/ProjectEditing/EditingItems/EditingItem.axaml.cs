using System;
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

namespace ProjectAvalonia.Project.Components.ProjectEditing.EditingItems;
public partial class EditingItem : ReactiveUserControl<ProjectItemEditingViewModel>
{

    public static readonly AttachedProperty<FileProjectItemViewModel> ItemProperty =
          AvaloniaProperty.RegisterAttached<EditingItem, ReactiveUserControl<ProjectItemEditingViewModel>, FileProjectItemViewModel>(
              nameof(Item)
              );
    public FileProjectItemViewModel? Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }
    public ScrollViewer editingScrollViewer => this.FindControl<ScrollViewer>("EditingScrollViewer");

    public EditingItem()
    {
        ViewModel ??= Locator.Current.GetService<ProjectItemEditingViewModel>();

        DataContext = ViewModel;

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            ViewModel.Activator.Activate();

            /* KeyBindings.Add(new Avalonia.Input.KeyBinding()
             {
                 Command = ViewModel.SaveItemCommand,
                 CommandParameter = ViewModel.Item,
                 Gesture = new(Avalonia.Input.Key.S, Avalonia.Input.KeyModifiers.Control)
             });*/

            this.WhenPropertyChanged(prop => prop.Item)
            .Subscribe(async prop =>
            {
                if (prop.Value is not null)
                {
                    await ViewModel.SetEditingItem(prop.Value.Path);
                    ViewModel.SelectedItem = prop.Value;
                    editingScrollViewer.ScrollToHome();
                }
            })
            .DisposeWith(disposables);

        });

        AvaloniaXamlLoader.Load(this);
    }
}
