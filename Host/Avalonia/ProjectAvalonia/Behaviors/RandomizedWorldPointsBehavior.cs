using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using ProjectAvalonia.Common.Logging;

namespace ProjectAvalonia.Behaviors;

public class RandomizedWorldPointsBehavior : Behavior<Canvas>
{
    private static readonly Random RandomSource = new();
    private CancellationTokenSource _cts = new();
    private List<Control> _targetControls = new(); // ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

    private static readonly List<Point> WorldLocations = new()
    {
        new(x: 816, y: 219), new(x: 855, y: 146), new(x: 816, y: 142), new(x: 812, y: 188), new(x: 780, y: 130)
        , new(x: 784, y: 194), new(x: 760, y: 258), new(x: 766, y: 286), new(x: 963, y: 406), new(x: 910, y: 365)
        , new(x: 898, y: 392), new(x: 869, y: 409), new(x: 791, y: 387), new(x: 677, y: 166), new(x: 677, y: 240)
        , new(x: 652, y: 194), new(x: 631, y: 118), new(x: 586, y: 182), new(x: 566, y: 263), new(x: 574, y: 340)
        , new(x: 531, y: 306), new(x: 537, y: 154), new(x: 543, y: 72), new(x: 509, y: 72), new(x: 472, y: 60)
        , new(x: 491, y: 106), new(x: 491, y: 160), new(x: 491, y: 392), new(x: 466, y: 96), new(x: 438, y: 246)
        , new(x: 438, y: 108), new(x: 426, y: 84), new(x: 400, y: 135), new(x: 406, y: 154), new(x: 377, y: 225)
        , new(x: 281, y: 351), new(x: 251, y: 275), new(x: 245, y: 391), new(x: 205, y: 409), new(x: 205, y: 123)
        , new(x: 205, y: 237), new(x: 193, y: 204), new(x: 179, y: 143), new(x: 173, y: 123), new(x: 173, y: 244)
        , new(x: 173, y: 306), new(x: 167, y: 177), new(x: 161, y: 95), new(x: 142, y: 222), new(x: 120, y: 76)
        , new(x: 120, y: 163), new(x: 118, y: 210), new(x: 76, y: 101), new(x: 57, y: 143)
    };

    private void RunAnimation(
        CancellationToken cancellationToken
    ) =>
        Task.Run(
            action: () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var locations = WorldLocations
                            .OrderBy(_ => RandomSource.NextDouble())
                            .Take(_targetControls.Count);

                        var cities = _targetControls.Zip(second: locations, resultSelector: (
                            control
                            , point
                        ) => (control, point));

                        Task.WaitAll(
                            tasks: cities.Select(x => AnimateCityMarkerAsync(target: x.control, point: x.point
                                , cancellationToken: cancellationToken)).ToArray(),
                            cancellationToken: cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        if (ex is OperationCanceledException)
                        {
                            return;
                        }

                        Logger.LogWarning(
                            $"There was a problem while animating in {nameof(RandomizedWorldPointsBehavior)}: '{ex}'.");
                    }
                }
            },
            cancellationToken: cancellationToken);

    private async Task AnimateCityMarkerAsync(
        Control target
        , Point point
        , CancellationToken cancellationToken
    )
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        await Dispatcher.UIThread.InvokeAsync(() =>
            target.SetValue(property: Visual.OpacityProperty, value: 0, priority: BindingPriority.StyleTrigger));

        await Task.Delay(delay: TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            target.SetValue(property: Canvas.LeftProperty, value: point.X, priority: BindingPriority.StyleTrigger);
            target.SetValue(property: Canvas.TopProperty, value: point.Y, priority: BindingPriority.StyleTrigger);
            target.SetValue(property: Visual.OpacityProperty, value: 1, priority: BindingPriority.StyleTrigger);
        });

        await Task.Delay(delay: TimeSpan.FromSeconds(2), cancellationToken: cancellationToken);

        await Dispatcher.UIThread.InvokeAsync(() =>
            target.SetValue(property: Visual.OpacityProperty, value: 0, priority: BindingPriority.StyleTrigger));
    }

    protected override void OnDetaching()
    {
        _cts.Cancel();
        _cts.Dispose();
        base.OnDetaching();
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        Dispatcher.UIThread.Post(
            action: () =>
            {
                if (AssociatedObject?.Children is null || AssociatedObject.Children.Count == 0)
                {
                    return;
                }

                var targets = AssociatedObject.Children
                    .Where(x => x.Classes.Contains("City"))
                    .ToList();

                if (targets.Count <= 0)
                {
                    return;
                }

                _targetControls = targets;
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                RunAnimation(_cts.Token);
            },
            priority: DispatcherPriority.Loaded);
    }
}