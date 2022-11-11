using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;
public partial class AddItemWindow : Window
{
    public AddItemWindow()
    {
       /* this.WhenActivated(disposables =>
        {
           
        });*/

        AvaloniaXamlLoader.Load(this);
    }
}
