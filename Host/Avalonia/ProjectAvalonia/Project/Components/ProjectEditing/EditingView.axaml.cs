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
        ViewModel = Locator.Current.GetService<ProjectEditingViewModel>();

        DataContext = ViewModel;

        this.WhenActivated(disposables =>
        {
        });

        AvaloniaXamlLoader.Load(this);
    }
}
