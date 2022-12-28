using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace ProjectWinUI.Src.App.Behaviors;
public class ConditionalRenderingBehavior : Behavior<UIElement>
{
    public UIElement OnSuccessContent
    {
        get => (UIElement)GetValue(OnSuccessContentProperty);
        set => SetValue(OnSuccessContentProperty, value);
    }
    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OnSuccessContentProperty =
        DependencyProperty.Register("OnSuccessContent", typeof(UIElement), typeof(ConditionalRenderingBehavior), new PropertyMetadata(null));

    public UIElement OnFailedContent
    {
        get => (UIElement)GetValue(OnFailedContentProperty);
        set => SetValue(OnFailedContentProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OnFailedContentProperty =
        DependencyProperty.Register("OnFailedContent", typeof(UIElement), typeof(ConditionalRenderingBehavior), new PropertyMetadata(null));

    public bool ConditionResult
    {
        get => (bool)GetValue(ConditionResultProperty);
        set => SetValue(ConditionResultProperty, value);
    }

    // Using a DependencyProperty as the backing store for ConditionResult.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ConditionResultProperty =
        DependencyProperty.Register("ConditionResult", typeof(bool?), typeof(ConditionalRenderingBehavior), new PropertyMetadata(null, (d, e) =>
        {
            if (e.NewValue is not null)
            {
                var parent = ((ConditionalRenderingBehavior)d).AssociatedObject;

                var thisElement = d as ConditionalRenderingBehavior;

                if ((bool)thisElement.GetValue(ConditionResultProperty))
                {
                    if (thisElement.OnSuccessContent is not null)
                        if (parent is Panel panel)
                        {
                            if (panel.Children.Count > 0)
                                panel.Children.Remove(thisElement.OnFailedContent);

                            panel.Children.Add(thisElement.OnSuccessContent);
                        }
                        else
                            (parent as dynamic).Content = thisElement.OnSuccessContent;
                }
                else
                {
                    if (thisElement.OnFailedContent is not null)
                        if (parent is Panel panel)
                        {
                            if (panel.Children.Count > 0)
                                panel.Children.Remove(thisElement.OnSuccessContent);


                            panel.Children.Add(thisElement.OnFailedContent);
                        }
                        else
                            (parent as dynamic).Content = thisElement.OnFailedContent;
                }
            }
        }));
}
