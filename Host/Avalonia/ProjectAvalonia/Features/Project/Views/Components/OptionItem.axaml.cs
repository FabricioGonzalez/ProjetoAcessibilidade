using System;
using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using DynamicData.Binding;

using ProjectAvalonia.Features.Project.States.FormItemState;

namespace ProjectAvalonia.Features.Project.Views.Components;
public partial class OptionItem : UserControl
{
    public static readonly AttachedProperty<bool> IsCheckedProperty =
        AvaloniaProperty.RegisterAttached<OptionItem, UserControl, bool>(nameof(IsChecked));
    public bool? IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
    public static readonly AttachedProperty<string> ValueProperty =
        AvaloniaProperty.RegisterAttached<OptionItem, UserControl, string>(nameof(Value));
    public string? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public OptionItem()
    {

        this.WhenPropertyChanged(v => v.IsChecked, notifyOnInitialValue: false)
        .Subscribe(OptionChecker);

        InitializeComponent();
    }

    private void OptionChecker(PropertyValue<OptionItem, bool?> prop)
    {
        if (prop.Value.GetValueOrDefault())
        {
            if ((prop.Sender.Parent.Parent as ItemsControl).ItemCount == 2)
            {
                foreach (OptionsItemState item in (prop.Sender.Parent.Parent as ItemsControl).Items)
                {
                    if (item.Value != (prop.Sender.DataContext as OptionsItemState).Value && item.IsChecked)
                    {
                        item.IsChecked = false;
                    }
                }
            }
            if ((prop.Sender.Parent.Parent as ItemsControl).ItemCount >= 4)
            {
                var list1 = new List<OptionsItemState>();
                var list2 = new List<OptionsItemState>();

                foreach (OptionsItemState item in (prop.Sender.Parent.Parent as ItemsControl).Items)
                {

                    if (list1.Count < ((prop.Sender.Parent.Parent as ItemsControl).ItemCount / 2))
                    {
                        list1.Add(item);
                    }
                    else
                    {
                        list2.Add(item);
                    }
                }
                list1.ForEach(i1 =>
                {
                    if (list1.Contains(prop.Sender.DataContext as OptionsItemState) && i1.Value != (prop.Sender.DataContext as OptionsItemState).Value && i1.IsChecked)
                    {
                        i1.IsChecked = false;
                    }

                });
                list2.ForEach(i2 =>
                {
                    if (list2.Contains(prop.Sender.DataContext as OptionsItemState) && i2.Value != (prop.Sender.DataContext as OptionsItemState).Value && i2.IsChecked)
                    {
                        i2.IsChecked = false;
                    }
                });
                list1.Clear();
                list2.Clear();
            }
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
