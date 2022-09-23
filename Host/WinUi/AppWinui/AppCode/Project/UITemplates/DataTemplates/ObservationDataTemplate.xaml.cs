using AppUsecases.Entities.AppFormDataItems.Observations;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.Project.UITemplates.DataTemplates;
public sealed partial class ObservationDataTemplate : UserControl
{
    public AppFormDataItemObservationModel ProjectItem
    {
        get => (AppFormDataItemObservationModel)GetValue(ProjectItemProperty);
        set => SetValue(ProjectItemProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectItemProperty =
        DependencyProperty.Register(nameof(ProjectItem),
            typeof(AppFormDataItemObservationModel),
            typeof(ObservationDataTemplate),
            new PropertyMetadata(null));


    public ObservationDataTemplate()
    {
        DataContext = ProjectItem;
        InitializeComponent();
    }
}
