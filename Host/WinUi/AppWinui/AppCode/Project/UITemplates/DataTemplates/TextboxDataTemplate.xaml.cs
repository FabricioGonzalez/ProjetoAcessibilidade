using AppUsecases.Entities.AppFormDataItems.Text;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.Project.UITemplates.DataTemplates;
public sealed partial class TextboxDataTemplate : UserControl
{
    public AppFormDataItemTextModel ProjectItem
    {
        get => (AppFormDataItemTextModel)GetValue(ProjectItemProperty);
        set => SetValue(ProjectItemProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectItemProperty =
        DependencyProperty.Register(nameof(ProjectItem),
            typeof(AppFormDataItemTextModel),
            typeof(TextboxDataTemplate),
            new PropertyMetadata(null));


    public TextboxDataTemplate()
    {
        DataContext = ProjectItem;
        InitializeComponent();
    }
}
