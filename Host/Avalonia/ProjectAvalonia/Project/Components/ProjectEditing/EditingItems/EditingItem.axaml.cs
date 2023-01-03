using System.Reactive.Disposables;

using AppViewModels.Project;
using AppViewModels.Project.ComposableViewModels;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Project.Components.ProjectEditing.EditingItems;
public partial class EditingItem : ReactiveUserControl<ProjectItemEditingViewModel>
{
    public static readonly DirectProperty<EditingItem, FileProjectItemViewModel> ItemProperty =
    AvaloniaProperty.RegisterDirect<EditingItem, FileProjectItemViewModel>(
        "Item",
        o => o.ViewModel.SelectedItem,
        (o, v) => o.ViewModel.SelectedItem = v);

    public EditingItem()
    {
        ViewModel ??= Locator.Current.GetService<ProjectItemEditingViewModel>();

        DataContext = ViewModel;

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            ViewModel.Activator.Activate();

        });

        AvaloniaXamlLoader.Load(this);
    }
}
