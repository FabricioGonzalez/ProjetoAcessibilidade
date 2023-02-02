using System;
using System.Reactive.Disposables;

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

using ProjectAvalonia.Common.Controls;
using ProjectAvalonia.Features.Project.ViewModels;

namespace ProjectAvalonia.Features.Project.Views;
public partial class ProjectExplorerView : UserControl
{
    public ToggleButton SolutionButton => this.FindControl<ToggleButton>("SolutionFlyoutButton");
    public ProjectExplorerView()
    {
        InitializeComponent();

        AddHandlers(new CompositeDisposable());
    }

    private void AddHandlers(CompositeDisposable disposables)
    {
        disposables.Add(
            item: Disposable.Create(
            dispose: () =>
            {
                (SolutionButton.Flyout as SolutionFlyout).Closed -= ExplorerComponent_Closed;
                /*(SolutionButton.Flyout as SolutionFlyout).Opened -= ExplorerComponent_Closed;*/
            }));

        (SolutionButton.Flyout as SolutionFlyout).Closed += ExplorerComponent_Closed;
        /*(SolutionButton.Flyout as SolutionFlyout).Opened += ExplorerComponent_Closed;*/
    }

    private void ExplorerComponent_Closed(object? sender, EventArgs e) =>
        (DataContext as ProjectExplorerViewModel).IsDocumentSolutionEnabled
        = (DataContext as ProjectExplorerViewModel).IsDocumentSolutionEnabled ? false : true;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
