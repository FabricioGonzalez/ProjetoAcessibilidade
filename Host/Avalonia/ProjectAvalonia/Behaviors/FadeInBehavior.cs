using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Styling;

namespace ProjectAvalonia.Behaviors;

public class FadeInBehavior : AttachedToVisualTreeBehavior<Visual>
{
    public static readonly StyledProperty<TimeSpan> InitialDelayProperty =
        AvaloniaProperty.Register<ItemsControlAnimationBehavior, TimeSpan>(name: nameof(InitialDelay)
            , defaultValue: TimeSpan.FromMilliseconds(value: 500));

    public static readonly StyledProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.Register<ItemsControlAnimationBehavior, TimeSpan>(name: nameof(Duration)
            , defaultValue: TimeSpan.FromMilliseconds(value: 250));

    public TimeSpan InitialDelay
    {
        get => GetValue(property: InitialDelayProperty);
        set => SetValue(property: InitialDelayProperty, value: value);
    }

    public TimeSpan Duration
    {
        get => GetValue(property: DurationProperty);
        set => SetValue(property: DurationProperty, value: value);
    }

    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposable
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var totalDuration = InitialDelay + Duration;

        var animation = new Animation
        {
            Duration = totalDuration, Children =
            {
                new KeyFrame
                {
                    KeyTime = TimeSpan.Zero, Setters =
                    {
                        new Setter(property: Visual.OpacityProperty, value: 0d)
                    }
                }
                , new KeyFrame
                {
                    KeyTime = InitialDelay, Setters =
                    {
                        new Setter(property: Visual.OpacityProperty, value: 0d)
                    }
                }
                , new KeyFrame
                {
                    KeyTime = Duration, Setters =
                    {
                        new Setter(property: Visual.OpacityProperty, value: 1d)
                    }
                }
            }
        };
        animation.RunAsync(control: AssociatedObject, clock: null);
    }
}