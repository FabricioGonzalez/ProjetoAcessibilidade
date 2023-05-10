using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using DynamicData.Kernel;
using ProjectAvalonia.Features.TemplateEdit.Views.Models;
using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.Views.Components;

public class ComboEditBox : UserControl
{
    public static readonly DirectProperty<ComboEditBox, IEnumerable<TypeTemplate>> ItemsProperty =
        AvaloniaProperty.RegisterDirect<ComboEditBox, IEnumerable<TypeTemplate>>(
            nameof(Items),
            x => x.Items,
            (
                x
                , v
            ) => x.Items = v
        );

    public static readonly DirectProperty<ComboEditBox, string> SelectedValueProperty =
        AvaloniaProperty.RegisterDirect<ComboEditBox, string>(
            nameof(SelectedValue),
            x => x.SelectedValue,
            (
                x
                , v
            ) => x.SelectedValue = v,
            defaultBindingMode: BindingMode.TwoWay
        );

    public static readonly DirectProperty<ComboEditBox, IControl> UiSelectedTemplateItemProperty =
        AvaloniaProperty.RegisterDirect<ComboEditBox, IControl>(
            nameof(UISelectedTemplateItem),
            x => x.UISelectedTemplateItem,
            (
                x
                , v
            ) => x.UISelectedTemplateItem = v,
            defaultBindingMode: BindingMode.TwoWay
        );

    private IEnumerable<TypeTemplate> _items;

    private string _selectedValue;
    private IControl _uISelectedTemplateItem;

    public ComboEditBox()
    {
        InitializeComponent();

        this.WhenAnyValue(v => v.SelectedValue)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(value =>
            {
                Items.FirstOrOptional(item => item.TypeName.Equals(value))
                    .IfHasValue(hasValue =>
                    {
                        UISelectedTemplateItem = hasValue.UIElement;
                    });
            });
    }

    public string SelectedValue
    {
        get => _selectedValue;
        set => SetAndRaise(SelectedValueProperty, ref _selectedValue, value);
    }

    public IControl UISelectedTemplateItem
    {
        get => _uISelectedTemplateItem;
        set => SetAndRaise(UiSelectedTemplateItemProperty, ref _uISelectedTemplateItem, value);
    }

    public IEnumerable<TypeTemplate> Items
    {
        get => _items;
        set => SetAndRaise(ItemsProperty, ref _items, value);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}