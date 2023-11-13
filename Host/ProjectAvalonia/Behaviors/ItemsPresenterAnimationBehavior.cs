using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;

namespace ProjectAvalonia.Behaviors;

public class ItemsControlAnimationBehavior : AttachedToVisualTreeBehavior<ItemsControl>
{
    public static readonly StyledProperty<TimeSpan> InitialDelayProperty =
        AvaloniaProperty.Register<ItemsControlAnimationBehavior, TimeSpan>(name: nameof(InitialDelay)
            , defaultValue: TimeSpan.FromMilliseconds(value: 500));

    public static readonly StyledProperty<TimeSpan> ItemDurationProperty =
        AvaloniaProperty.Register<ItemsControlAnimationBehavior, TimeSpan>(name: nameof(ItemDuration)
            , defaultValue: TimeSpan.FromMilliseconds(value: 10));

    public TimeSpan InitialDelay
    {
        get => GetValue(property: InitialDelayProperty);
        set => SetValue(property: InitialDelayProperty, value: value);
    }

    public TimeSpan ItemDuration
    {
        get => GetValue(property: ItemDurationProperty);
        set => SetValue(property: ItemDurationProperty, value: value);
    }

    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposable
    )
    {
        if (AssociatedObject is null)
        {
        }

        /*Observable
            .FromEventPattern<ItemContainerEventArgs>(target: AssociatedObject.ItemContainerGenerator
                , eventName: nameof(ItemsControl.))
            .Select(selector: x => x.EventArgs)
            .Subscribe(onNext: e =>
            {
                foreach (var c in e.Containers)
                {
                    if (c.ContainerControl is not Visual v)
                    {
                        continue;
                    }

                    var duration = ItemDuration * (c.Index + 1);
                    var totalDuration = InitialDelay + duration * 2;

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
                                KeyTime = duration + InitialDelay, Setters =
                                {
                                    new Setter(property: Visual.OpacityProperty, value: 0d)
                                }
                            }
                            , new KeyFrame
                            {
                                KeyTime = totalDuration, Setters =
                                {
                                    new Setter(property: Visual.OpacityProperty, value: 1d)
                                }
                            }
                        }
                    };
                    animation.RunAsync(control: v);
                }
            })
            .DisposeWith(compositeDisposable: disposable);*/
    }
}