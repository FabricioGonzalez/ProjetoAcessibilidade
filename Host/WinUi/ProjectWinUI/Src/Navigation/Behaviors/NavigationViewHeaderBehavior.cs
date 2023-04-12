using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Xaml.Interactivity;
using ProjectWinUI.Src.Navigation.Contracts;
using ProjectWinUI.Src.Navigation.Enums;

namespace ProjectWinUI.Src.Navigation.Behaviors;

public class NavigationViewHeaderBehavior : Behavior<NavigationView>
{
    private static NavigationViewHeaderBehavior? _current;

    public static readonly DependencyProperty DefaultHeaderProperty =
        DependencyProperty.Register(name: "DefaultHeader",
            propertyType: typeof(object),
            ownerType: typeof(NavigationViewHeaderBehavior),
            typeMetadata: new PropertyMetadata(defaultValue: null,
                propertyChangedCallback: (
                    d
                    , e
                ) => _current!.UpdateHeader()));

    public static readonly DependencyProperty HeaderModeProperty =
        DependencyProperty.RegisterAttached(name: "HeaderMode", propertyType: typeof(bool),
            ownerType: typeof(NavigationViewHeaderBehavior),
            defaultMetadata: new PropertyMetadata(defaultValue: NavigationViewHeaderMode.Never,
                propertyChangedCallback: (
                    d
                    , e
                ) => _current!.UpdateHeader()));

    public static readonly DependencyProperty HeaderContextProperty =
        DependencyProperty.RegisterAttached(name: "HeaderContext",
            propertyType: typeof(object),
            ownerType: typeof(NavigationViewHeaderBehavior), defaultMetadata: new PropertyMetadata(defaultValue: null,
                propertyChangedCallback: (
                    d
                    , e
                ) => _current!.UpdateHeader()));

    public static readonly DependencyProperty HeaderTemplateProperty =
        DependencyProperty.RegisterAttached(name: "HeaderTemplate",
            propertyType: typeof(DataTemplate),
            ownerType: typeof(NavigationViewHeaderBehavior),
            defaultMetadata: new PropertyMetadata(defaultValue: null,
                propertyChangedCallback: (
                    d
                    , e
                ) => _current!.UpdateHeaderTemplate()));

    private Page? _currentPage;

    public DataTemplate? DefaultHeaderTemplate
    {
        get;
        set;
    }

    public object DefaultHeader
    {
        get => GetValue(dp: DefaultHeaderProperty);
        set => SetValue(dp: DefaultHeaderProperty, value: value);
    }

    public static NavigationViewHeaderMode GetHeaderMode(
        Page item
    ) => (NavigationViewHeaderMode)item.GetValue(dp: HeaderModeProperty);

    public static void SetHeaderMode(
        Page item
        , NavigationViewHeaderMode value
    ) => item.SetValue(dp: HeaderModeProperty, value: value);

    public static object GetHeaderContext(
        Page item
    ) => item.GetValue(dp: HeaderContextProperty);

    public static void SetHeaderContext(
        Page item
        , object value
    ) => item.SetValue(dp: HeaderContextProperty, value: value);

    public static DataTemplate GetHeaderTemplate(
        Page item
    ) => (DataTemplate)item.GetValue(dp: HeaderTemplateProperty);

    public static void SetHeaderTemplate(
        Page item
        , DataTemplate value
    ) => item.SetValue(dp: HeaderTemplateProperty, value: value);

    protected override void OnAttached()
    {
        base.OnAttached();

        var navigationService = WinApp.GetService<INavigationService>();
        navigationService.Navigated += OnNavigated;

        _current = this;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        var navigationService = WinApp.GetService<INavigationService>();
        navigationService.Navigated -= OnNavigated;
    }

    private void OnNavigated(
        object sender
        , NavigationEventArgs e
    )
    {
        if (sender is Frame frame && frame.Content is Page page)
        {
            _currentPage = page;

            UpdateHeader();
            UpdateHeaderTemplate();
        }
    }

    private void UpdateHeader()
    {
        if (_currentPage != null)
        {
            var headerMode = GetHeaderMode(item: _currentPage);
            if (headerMode == NavigationViewHeaderMode.Never)
            {
                AssociatedObject.Header = null;
                AssociatedObject.AlwaysShowHeader = false;
            }
            else
            {
                var headerFromPage = GetHeaderContext(item: _currentPage);
                if (headerFromPage != null)
                {
                    AssociatedObject.Header = headerFromPage;
                }
                else
                {
                    AssociatedObject.Header = DefaultHeader;
                }

                if (headerMode == NavigationViewHeaderMode.Always)
                {
                    AssociatedObject.AlwaysShowHeader = true;
                }
                else
                {
                    AssociatedObject.AlwaysShowHeader = false;
                }
            }
        }
    }

    private void UpdateHeaderTemplate()
    {
        if (_currentPage != null)
        {
            var headerTemplate = GetHeaderTemplate(item: _currentPage);
            AssociatedObject.HeaderTemplate = headerTemplate ?? DefaultHeaderTemplate;
        }
    }
}