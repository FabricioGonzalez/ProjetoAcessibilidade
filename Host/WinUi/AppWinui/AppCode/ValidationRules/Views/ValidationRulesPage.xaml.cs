using AppUsecases.Entities;

using AppWinui.AppCode.AppUtils.Behaviors;
using AppWinui.AppCode.TemplateEditing.ViewModels;

using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.ValidationRules.Views;

public sealed partial class ValidationRulesPage : Page
{
    public TemplateEditViewModel ViewModel
    {
        get;
    }

    public ValidationRulesPage()
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
          typeof(ValidationRulesPage),
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
            typeof(ValidationRulesPage),
            new PropertyMetadata(new AppItemModel()));

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
