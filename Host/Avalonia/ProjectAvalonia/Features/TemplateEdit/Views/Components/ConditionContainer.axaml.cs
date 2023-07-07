using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.FormItemState;
using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.Views.Components;

public partial class ConditionContainer : UserControl
{
    public ConditionContainer()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void SelectingItemsControl_OnSelectionChanged(
        object? sender
        , SelectionChangedEventArgs e
    )
    {
        if (DataContext is IConditionState condition)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems.Count < 2)
            {
                condition.TargetId = (e.AddedItems[0] as OptionsItemState)?.Id ?? "";
            }
        }
        
    }
}