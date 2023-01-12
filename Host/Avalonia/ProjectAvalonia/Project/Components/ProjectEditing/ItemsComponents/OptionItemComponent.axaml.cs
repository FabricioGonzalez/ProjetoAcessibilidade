using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Disposables;

using AppViewModels.Project.States.ProjectItemState.FormItemState;

using Avalonia;

using Avalonia.Controls;

using DynamicData.Binding;

using ReactiveUI;

namespace ProjectAvalonia.Project.Components.ProjectEditing.ItemsComponents;
public partial class OptionItemComponent : UserControl, IActivatableView
{
    public static readonly AttachedProperty<bool> IsCheckedProperty =
        AvaloniaProperty.RegisterAttached<OptionItemComponent, UserControl, bool>(nameof(IsChecked));
    public bool? IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
    public static readonly AttachedProperty<string> ValueProperty =
        AvaloniaProperty.RegisterAttached<OptionItemComponent, UserControl, string>(nameof(Value));
    public string? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public OptionItemComponent()
    {
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.WhenPropertyChanged(v => v.IsChecked, notifyOnInitialValue: false)
            .Subscribe(prop =>
            {
                OptionChecker(prop);
            });
        });

        InitializeComponent();
    }

    private void OptionChecker(PropertyValue<OptionItemComponent, bool?> prop)
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

                        Debug.WriteLine(item.IsChecked.ToString(), $"2 Items: {item.Value}");
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

                        Debug.WriteLine(i1.IsChecked.ToString(), $"Group 1: {i1.Value}");
                    }

                });
                list2.ForEach(i2 =>
                {
                    if (list2.Contains(prop.Sender.DataContext as OptionsItemState) && i2.Value != (prop.Sender.DataContext as OptionsItemState).Value && i2.IsChecked)
                    {
                        i2.IsChecked = false;

                        Debug.WriteLine(i2.IsChecked.ToString(), $"Group 2: {i2.Value}");
                    }
                });
                list1.Clear();
                list2.Clear();
            }
        }
    }
}
