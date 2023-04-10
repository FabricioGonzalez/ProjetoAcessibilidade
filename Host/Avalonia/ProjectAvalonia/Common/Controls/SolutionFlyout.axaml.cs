using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectAvalonia.Features.Project.ViewModels;
using ProjectAvalonia.Features.Project.Views;

namespace ProjectAvalonia.Common.Controls;

public class SolutionFlyout : Flyout
{
    public static readonly AttachedProperty<SolutionStateViewModel> SolutionModelProperty =
        AvaloniaProperty.RegisterAttached<SolutionFlyout, ProjectExplorerView, SolutionStateViewModel>(
            name: nameof(SolutionModel));

    public SolutionFlyout()
    {
        InitializeComponent();
    }

    public SolutionStateViewModel SolutionModel
    {
        get => GetValue(property: SolutionModelProperty);
        set => SetValue(property: SolutionModelProperty, value: value);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(obj: this);
}