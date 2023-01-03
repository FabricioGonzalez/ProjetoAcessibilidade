using System.Reactive.Disposables;

using AppViewModels.Project;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

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
