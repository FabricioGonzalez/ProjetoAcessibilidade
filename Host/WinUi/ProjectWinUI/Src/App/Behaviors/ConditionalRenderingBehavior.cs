using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace ProjectWinUI.Src.App.Behaviors;

public class ConditionalRenderingBehavior : Behavior<UIElement>
{
    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OnSuccessContentProperty =
        DependencyProperty.Register(name: "OnSuccessContent", propertyType: typeof(UIElement)
            , ownerType: typeof(ConditionalRenderingBehavior), typeMetadata: new PropertyMetadata(defaultValue: null));

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OnFailedContentProperty =
        DependencyProperty.Register(name: "OnFailedContent", propertyType: typeof(UIElement)
            , ownerType: typeof(ConditionalRenderingBehavior), typeMetadata: new PropertyMetadata(defaultValue: null));

    // Using a DependencyProperty as the backing store for ConditionResult.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ConditionResultProperty =
        DependencyProperty.Register(name: "ConditionResult", propertyType: typeof(bool?)
            , ownerType: typeof(ConditionalRenderingBehavior), typeMetadata: new PropertyMetadata(defaultValue: null
                , propertyChangedCallback: (
                    d
                    , e
                ) =>
                {
                    if (e.NewValue is not null)
                    {
                        var parent = ((ConditionalRenderingBehavior)d).AssociatedObject;

                        var thisElement = d as ConditionalRenderingBehavior;

                        if ((bool)thisElement.GetValue(dp: ConditionResultProperty))
                        {
                            if (thisElement.OnSuccessContent is not null)
                            {
                                if (parent is Panel panel)
                                {
                                    if (panel.Children.Count > 0)
                                    {
                                        panel.Children.Remove(item: thisElement.OnFailedContent);
                                    }

                                    panel.Children.Add(item: thisElement.OnSuccessContent);
                                }
                                else
                                {
                                    (parent as dynamic).Content = thisElement.OnSuccessContent;
                                }
                            }
                        }
                        else
                        {
                            if (thisElement.OnFailedContent is not null)
                            {
                                if (parent is Panel panel)
                                {
                                    if (panel.Children.Count > 0)
                                    {
                                        panel.Children.Remove(item: thisElement.OnSuccessContent);
                                    }


                                    panel.Children.Add(item: thisElement.OnFailedContent);
                                }
                                else
                                {
                                    (parent as dynamic).Content = thisElement.OnFailedContent;
                                }
                            }
                        }
                    }
                }));

    public UIElement OnSuccessContent
    {
        get => (UIElement)GetValue(dp: OnSuccessContentProperty);
        set => SetValue(dp: OnSuccessContentProperty, value: value);
    }

    public UIElement OnFailedContent
    {
        get => (UIElement)GetValue(dp: OnFailedContentProperty);
        set => SetValue(dp: OnFailedContentProperty, value: value);
    }

    public bool ConditionResult
    {
        get => (bool)GetValue(dp: ConditionResultProperty);
        set => SetValue(dp: ConditionResultProperty, value: value);
    }
}