using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Project.Components.ProjectEditing;
public partial class EditingView : UserControl
{
    public EditingView()
    {

       /* this.WhenActivated(disposables =>
        {
            ViewModel = Locator.Current.GetService<ExplorerComponentViewModel>();
        });*/

        this.DataContext = new TabItemModelTest[] {
    new TabItemModelTest("One", "Some content on first tab"),
    new TabItemModelTest("Two", "Some content on second tab"),
};

        AvaloniaXamlLoader.Load(this);
    }
}
