using AppUsecases.Entities;

using AppWinui.AppCode.AppUtils.Behaviors;
using AppWinui.AppCode.TemplateEditing.ViewModels;

using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.TemplateEditing.Views;

public sealed partial class ListDetailsPage : Page
{
    public TemplateEditViewModel ViewModel
    {
        get;
    }

    public ListDetailsPage()
    {
        ViewModel = App.GetService<TemplateEditViewModel>();
        InitializeComponent();
    }

    public NavigationViewHeaderMode HeaderMode
    {
        get => (NavigationViewHeaderMode)GetValue(HeaderModeProperty);
        set => SetValue(HeaderModeProperty, value);
    }

    public static readonly DependencyProperty HeaderModeProperty =
      DependencyProperty.RegisterAttached("HeaderMode", typeof(bool),
          typeof(ListDetailsPage),
          new PropertyMetadata(NavigationViewHeaderMode.Never));

    public AppItemModel ProjectItem
    {
        get => (AppItemModel)GetValue(ProjectItemProperty);
        set => SetValue(ProjectItemProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectItemProperty =
        DependencyProperty.Register("ProjectItem", 
            typeof(AppItemModel),
            typeof(ListDetailsPage),
            new PropertyMetadata(new AppItemModel()));

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
