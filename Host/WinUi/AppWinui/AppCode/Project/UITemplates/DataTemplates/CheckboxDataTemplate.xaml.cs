using AppUsecases.Entities.AppFormDataItems.Checkbox;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.Project.UITemplates.DataTemplates;
public sealed partial class CheckboxDataTemplate : UserControl
{


    public AppFormDataItemCheckboxModel ProjectItem
    {
        get => (AppFormDataItemCheckboxModel)GetValue(ProjectItemProperty);
        set => SetValue(ProjectItemProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectItemProperty =
        DependencyProperty.Register(nameof(ProjectItem), 
            typeof(AppFormDataItemCheckboxModel),
            typeof(CheckboxDataTemplate), 
            new PropertyMetadata(null));


    public CheckboxDataTemplate()
    {
        DataContext = ProjectItem;
        InitializeComponent();
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {

    }
}
