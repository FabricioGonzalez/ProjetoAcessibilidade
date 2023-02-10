using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using ProjectAvalonia.Features.Project.ViewModels;
using ProjectAvalonia.Features.Project.Views;

namespace ProjectAvalonia.Common.Controls;
public partial class SolutionFlyout : Flyout
{

    public static readonly AttachedProperty<SolutionStateViewModel> SolutionModelProperty =
         AvaloniaProperty.RegisterAttached<SolutionFlyout, ProjectExplorerView, SolutionStateViewModel>(
             nameof(SolutionModel));
    public SolutionStateViewModel SolutionModel
    {
        get => GetValue(SolutionModelProperty);
        set => SetValue(SolutionModelProperty, value);
    }

    public SolutionFlyout()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
