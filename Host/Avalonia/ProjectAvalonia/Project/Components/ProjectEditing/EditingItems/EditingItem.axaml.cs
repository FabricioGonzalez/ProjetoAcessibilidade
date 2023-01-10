using System;
using System.Reactive.Disposables;
using System.Windows.Input;

using AppViewModels.Project;
using AppViewModels.Project.ComposableViewModels;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DynamicData.Binding;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Project.Components.ProjectEditing.EditingItems;
public partial class EditingItem : ReactiveUserControl<ProjectItemEditingViewModel>, ICommandSource
{

    public static readonly AttachedProperty<FileProjectItemViewModel> ItemProperty =
          AvaloniaProperty.RegisterAttached<EditingItem, ReactiveUserControl<ProjectItemEditingViewModel>, FileProjectItemViewModel>(nameof(Item));
    public FileProjectItemViewModel? Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public ICommand? Command => this?.ViewModel?.SaveItemCommand;
    public object? CommandParameter => this?.ViewModel?.Item;

    public ScrollViewer editingScrollViewer => this.FindControl<ScrollViewer>("EditingScrollViewer");

    public EditingItem()
    {
        ViewModel ??= Locator.Current.GetService<ProjectItemEditingViewModel>();

        HotKeyManager.SetHotKey(this, new KeyGesture(Key.S, KeyModifiers.Control));

        DataContext = ViewModel;

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            ViewModel.Activator.Activate();

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

    public void CanExecuteChanged(object sender, EventArgs e)
    {

    }
}
