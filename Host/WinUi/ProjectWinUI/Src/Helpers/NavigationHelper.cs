using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ProjectWinUI.Src.Helpers;

public class NavigationHelper
{
    public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached(name: "NavigateTo", propertyType: typeof(string)
            , ownerType: typeof(NavigationHelper), defaultMetadata: new PropertyMetadata(defaultValue: null));

    public static readonly DependencyProperty NavigateToParameterProperty =
        DependencyProperty.RegisterAttached(
            name: "NavigationParameter",
            propertyType: typeof(object),
            ownerType: typeof(NavigationHelper),
            defaultMetadata: new PropertyMetadata(defaultValue: null));

    public static string GetNavigateTo(
        NavigationViewItem item
    ) => (string)item.GetValue(dp: NavigateToProperty);

    public static void SetNavigateTo(
        NavigationViewItem item
        , string value
    ) => item.SetValue(dp: NavigateToProperty, value: value);

    public static object GetParameter(
        NavigationViewItem item
    )
        => item.GetValue(dp: NavigateToParameterProperty);

    public static void SetParameter(
        NavigationViewItem item
        , object value
    )
        => item.SetValue(dp: NavigateToParameterProperty, value: value);
}