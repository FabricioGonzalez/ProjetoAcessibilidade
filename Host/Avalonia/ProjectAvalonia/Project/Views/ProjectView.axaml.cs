using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels;

using ReactiveUI;

namespace ProjectAvalonia.Project.Views;
public partial class ProjectView : ReactiveUserControl<ProjectViewModel>
{
    public static readonly DirectProperty<ProjectView, string> ProjectPathProperty =
       AvaloniaProperty.RegisterDirect<ProjectView, string>(
           nameof(ProjectPath),
           owner => owner.ViewModel.strFolder,
           (owner, value) => owner.ViewModel.strFolder = value);
   
    private string _projectPath = "";
    public string ProjectPath
    {
        get => _projectPath;
        set => SetAndRaise(ProjectPathProperty, ref _projectPath, value);
    }
    public ProjectView()
    {

        this.WhenActivated(disposables => {

        });

        AvaloniaXamlLoader.Load(this);
    }
}
