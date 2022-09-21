using AppWinui.AppCode.AppUtils.Behaviors;
using AppWinui.AppCode.TemplateEditing.ViewModels;

using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.TemplateEditing.Views;

public sealed partial class ListDetailsPage : Page
{
    public ListDetailsViewModel ViewModel
    {
        get;
    }

    public ListDetailsPage()
    {
        ViewModel = App.GetService<ListDetailsViewModel>();
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

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
