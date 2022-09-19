using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;

namespace AppWinui.AppCode.AppUtils.Behaviors;
public class ConditionalRenderingBehavior : Behavior<UIElement>
{
    private static ConditionalRenderingBehavior? _current;
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
        DependencyProperty.Register("ConditionResult", typeof(bool), typeof(ConditionalRenderingBehavior), new PropertyMetadata(true, (d, e) =>
        {
           var parent = VisualTreeHelper.GetParent(d) as UIElement;

            var thisElement = d as ConditionalRenderingBehavior;

            if ((bool)thisElement.GetValue(ConditionResultProperty))
            {

            }
            else
            {
                
            }

        }));
    //private void UpdateState()
    //{
        
    //}
}
