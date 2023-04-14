using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Views.Components;

public partial class OptionItem : UserControl
{
    public static readonly AttachedProperty<bool> IsCheckedProperty =
        AvaloniaProperty.RegisterAttached<OptionItem, UserControl, bool>(name: nameof(IsChecked));

    public static readonly AttachedProperty<string> ValueProperty =
        AvaloniaProperty.RegisterAttached<OptionItem, UserControl, string>(name: nameof(Value));

    public OptionItem()
    {
        /*this.WhenPropertyChanged(propertyAccessor: v => v.IsChecked, notifyOnInitialValue: false)
            .Subscribe(onNext: OptionChecker);*/

        InitializeComponent();
    }

    public bool? IsChecked
    {
        get => GetValue(property: IsCheckedProperty);
        set => SetValue(property: IsCheckedProperty, value: value);
    }

    public string? Value
    {
        get => GetValue(property: ValueProperty);
        set => SetValue(property: ValueProperty, value: value);
    }

    /*private void OptionChecker(
        PropertyValue<OptionItem, bool?> prop
    )
    {
        if (prop.Value.GetValueOrDefault())
        {
            if ((prop.Sender?.Parent?.Parent as ItemsControl)?.ItemCount == 2)
            {
                foreach (OptionsItemState item in (prop.Sender.Parent.Parent as ItemsControl).Items)
                {
                    if (item.Value != (prop.Sender.DataContext as OptionsItemState).Value && item.IsChecked)
                    {
                        item.IsChecked = false;
                    }
                }
            }

            if ((prop.Sender?.Parent?.Parent as ItemsControl)?.ItemCount >= 4)
            {
                var list1 = new List<OptionsItemState>();
                var list2 = new List<OptionsItemState>();

                foreach (OptionsItemState item in (prop.Sender.Parent.Parent as ItemsControl).Items)
                {
                    if (list1.Count < (prop.Sender.Parent.Parent as ItemsControl).ItemCount / 2)
                    {
                        list1.Add(item: item);
                    }
                    else
                    {
                        list2.Add(item: item);
                    }
                }

                list1.ForEach(action: i1 =>
                {
                    if (list1.Contains(item: prop.Sender.DataContext as OptionsItemState)
                        && i1.Value != (prop.Sender.DataContext as OptionsItemState).Value && i1.IsChecked)
                    {
                        i1.IsChecked = false;
                    }
                });
                list2.ForEach(action: i2 =>
                {
                    if (list2.Contains(item: prop.Sender.DataContext as OptionsItemState)
                        && i2.Value != (prop.Sender.DataContext as OptionsItemState).Value && i2.IsChecked)
                    {
                        i2.IsChecked = false;
                    }
                });
                list1.Clear();
                list2.Clear();
            }
        }
    }
    */

    private void InitializeComponent() => AvaloniaXamlLoader.Load(obj: this);
}