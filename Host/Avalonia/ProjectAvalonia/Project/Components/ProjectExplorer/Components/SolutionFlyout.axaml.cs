using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Project.Components.ProjectExplorer;
public partial class SolutionFlyout : Flyout
{
    public SolutionFlyout()
    {
       
        AvaloniaXamlLoader.Load(this);
    }
}
