using System.Numerics;

using AppWinui.AppCode.Project.ViewModels;

using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;

using ReactiveUI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.Project.UIComponents.TabView;
public sealed partial class ProjectEditingTabViewItem : UserControl/*ReactiveUserControl<ProjectItemViewModel>*/
{
    private void ProjectEditingTabViewItem_Loaded(object sender, RoutedEventArgs e)
    {
        CompositionPropertySet scrollerPropertySet = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(Scroller);
        Compositor compositor = scrollerPropertySet.Compositor;

        // Get the visual that represents our HeaderTextBlock 
        // And define the progress animation string
        var headerVisual = ElementCompositionPreview.GetElementVisual(ScrollerHeader);
        var progress = "Clamp(Abs(scroller.Translation.Y) / 100.0, 0.0, 1.0)";

        // Create the expression and add in our progress string.
        var textExpression = compositor.CreateExpressionAnimation("Lerp(1.5, 1, " + progress + ")");
        textExpression.SetReferenceParameter("scroller", scrollerPropertySet);

        // Shift the header by 50 pixels when scrolling down
        var offsetExpression = compositor.CreateExpressionAnimation($"-scroller.Translation.Y - {progress} * 50");
        offsetExpression.SetReferenceParameter("scroller", scrollerPropertySet);
        headerVisual.StartAnimation("Offset.Y", offsetExpression);

        Visual textVisual = ElementCompositionPreview.GetElementVisual(commandBar);
        Vector3 finalOffset = new Vector3(0, 48, 0);
        var headerOffsetAnimation = compositor.CreateExpressionAnimation($"Lerp(Vector3(0,0,0), finalOffset, {progress})");
        headerOffsetAnimation.SetReferenceParameter("scroller", scrollerPropertySet);
        headerOffsetAnimation.SetVector3Parameter("finalOffset", finalOffset);
        textVisual.StartAnimation(nameof(Visual.Offset), headerOffsetAnimation);
    }


    public string ProjectItemPath
    {
        get => (string)GetValue(ProjectItemPathProperty);
        set => SetValue(ProjectItemPathProperty, value);
    }

    // Using a DependencyProperty as the backing store for ProjectItemPath.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectItemPathProperty =
        DependencyProperty.Register(nameof(ProjectItemPath), typeof(string), typeof(ProjectEditingTabViewItem), new PropertyMetadata(null));

    private ProjectItemViewModel ViewModel
    {
        get;
    }

    /*public IProjectItemCommands Commands
    {
        get => (IProjectItemCommands)GetValue(CommandsProperty);
        set => SetValue(CommandsProperty, value);
    }

    Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CommandsProperty =
        DependencyProperty.Register(nameof(Commands), typeof(IProjectItemCommands), typeof(ProjectEditingTabViewItem), new PropertyMetadata(null));
*/

    public ProjectEditingTabViewItem()
    {
        DataContext = App.GetService<ProjectItemViewModel>();
        ViewModel = (ProjectItemViewModel)DataContext;

       /* this.WhenActivated(disposableRegistration =>
        {
            this.BindCommand(ViewModel,
            viewModel => viewModel.OpenProjectItemCommand,
            view => view.tabItem)
            .DisposeWith(disposableRegistration);
        });*/

        InitializeComponent();
    }
}
