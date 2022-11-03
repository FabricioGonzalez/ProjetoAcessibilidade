using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels;

using ReactiveUI;

namespace ProjectAvalonia.Project.Views;
public partial class ProjectView : ReactiveUserControl<ProjectViewModel>
{
    public ProjectView()
    {

        this.WhenActivated(disposables => {

        });

        AvaloniaXamlLoader.Load(this);
    }
}
