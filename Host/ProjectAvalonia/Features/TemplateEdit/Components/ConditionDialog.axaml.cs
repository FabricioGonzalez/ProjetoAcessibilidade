using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.FormItemState;

namespace ProjectAvalonia.Features.TemplateEdit.Components;

public partial class ConditionDialog : UserControl
{
    public ConditionDialog()
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

    private void OptionsSelector_OnInitialized(
        object? sender
        , EventArgs e
    )
    {
        if (sender is ComboBox combo && DataContext is IConditionState condition)
        {
            combo.SelectedItem = combo.Items.Cast<OptionsItemState>().FirstOrDefault(it => it.Id == condition.TargetId);
        }
    }

    private void StyledElement_OnInitialized(
        object? sender
        , EventArgs e
    )
    {
        if (sender is ComboBox combo && DataContext is IConditionState condition)
        {
            combo.SelectedItem = combo.Items.Cast<ICheckingValue>()
                .FirstOrDefault(it => it.Value == condition.CheckingValue.Value);
        }
    }
}