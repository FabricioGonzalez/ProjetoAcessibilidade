using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace ProjectAvalonia.Common.Controls;

public class TagControl : ContentControl
{
    public static readonly StyledProperty<bool> EnableCounterProperty =
        AvaloniaProperty.Register<TagControl, bool>(name: nameof(EnableCounter));

    public static readonly StyledProperty<bool> EnableDeleteProperty =
        AvaloniaProperty.Register<TagControl, bool>(name: nameof(EnableDelete));

    public static readonly StyledProperty<int> OrdinalIndexProperty =
        AvaloniaProperty.Register<TagControl, int>(name: nameof(OrdinalIndex));

    private TagsBox? _parentTagBox;
    private IDisposable? _subscription;

    public bool EnableCounter
    {
        get => GetValue(property: EnableCounterProperty);
        set => SetValue(property: EnableCounterProperty, value: value);
    }

    public bool EnableDelete
    {
        get => GetValue(property: EnableDeleteProperty);
        set => SetValue(property: EnableDeleteProperty, value: value);
    }

    public int OrdinalIndex
    {
        get => GetValue(property: OrdinalIndexProperty);
        set => SetValue(property: OrdinalIndexProperty, value: value);
    }

    protected override void OnApplyTemplate(
        TemplateAppliedEventArgs e
    )
    {
        base.OnApplyTemplate(e: e);

        _parentTagBox = this.FindLogicalAncestorOfType<TagsBox>();

        var deleteButton = e.NameScope.Find<Button>(name: "PART_DeleteButton");

        if (deleteButton is null)
        {
            return;
        }

        deleteButton.Click += OnDeleteTagClicked;

        _subscription?.Dispose();
        _subscription = Disposable.Create(dispose: () => deleteButton.Click -= OnDeleteTagClicked);
    }

    private void OnDeleteTagClicked(
        object? sender
        , RoutedEventArgs e
    ) => _parentTagBox?.RemoveAt(index: OrdinalIndex - 1);
}