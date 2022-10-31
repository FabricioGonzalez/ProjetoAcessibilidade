using AppUsecases.Project.Entities.Project;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.TemplateEditing.UIComponents;

public sealed partial class ListDetailsDetailControl : UserControl
{

    public AppItemModel Item
    {
        get => GetValue(ItemProperty) as AppItemModel;
        set => SetValue(ItemProperty, value);
    }

    public static readonly DependencyProperty ItemProperty =
        DependencyProperty.Register(nameof(Item),
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
