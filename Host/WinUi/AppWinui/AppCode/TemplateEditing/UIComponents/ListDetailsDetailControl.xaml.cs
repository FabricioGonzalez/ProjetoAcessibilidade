using AppUsecases.Entities;

using AppWinui.AppCode.TemplateEditing.ViewModels;
using AppWinui.Core.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using ReactiveUI;

namespace AppWinui.AppCode.TemplateEditing.UIComponents;

public sealed partial class ListDetailsDetailControl : UserControl, IViewFor<ItemTemplateEditingViewModel>
{
    public ItemTemplateEditingViewModel ViewModel
    {
        get => (ItemTemplateEditingViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel),
            typeof(ItemTemplateEditingViewModel),
            typeof(ListDetailsDetailControl),
            new PropertyMetadata(null));

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ItemTemplateEditingViewModel)value;
    }


    public AppItemModel? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as AppItemModel;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty =
        DependencyProperty.Register(nameof(ListDetailsMenuItem),
            typeof(AppItemModel),
            typeof(ListDetailsDetailControl),
            new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public ListDetailsDetailControl()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ListDetailsDetailControl control)
        {
            /*control.ForegroundElement.ChangeView(0, 0, 1);*/
        }
    }
}
