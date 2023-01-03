using System.Reactive.Disposables;

using AppViewModels.Project;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Project.Components.ProjectEditing.EditingItems;
public partial class EditingItem : ReactiveUserControl<ProjectItemEditingViewModel>
{
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
