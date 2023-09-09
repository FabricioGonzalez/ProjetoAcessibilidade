using System;
using System.Threading;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace ProjectAvalonia.Common.Models;

public class NavBarSelectedIndicatorState : IDisposable
{
    private readonly Easing _bckEasing = new SplineEasing(x1: 0.2, y1: 1, x2: 0.1, y2: 0.9);
    private readonly Easing _fwdEasing = new SplineEasing(x1: 0.1, y1: 0.9, x2: 0.2);
    private readonly TimeSpan _totalDuration = TimeSpan.FromSeconds(value: 0.4);
    private Control? _activeIndicator;

    private CancellationTokenSource _currentAnimationCts = new();

    private bool _isDisposed;
    private bool _previousAnimationOngoing;

    // This will be used in the future for horizontal selection indicators.
    public Orientation NavItemsOrientation
    {
        get;
        set;
    } = Orientation.Vertical;

    public void Dispose()
    {
        _isDisposed = true;
        CancelPriorAnimation();
    }

    private void CancelPriorAnimation()
    {
        _currentAnimationCts.Cancel();
        _currentAnimationCts.Dispose();
        _currentAnimationCts = new CancellationTokenSource();
    }

    private static Matrix GetOffsetFrom(
        Visual ancestor
        , Visual visual
    )
    {
        var identity = Matrix.Identity;
        while (visual != ancestor)
        {
            var bounds = visual.Bounds;
            var topLeft = bounds.TopLeft;

            if (topLeft != new Point())
            {
                identity *= Matrix.CreateTranslation(position: topLeft);
            }

            if (visual.Parent is null)
            {
                return Matrix.Identity;
            }

            visual = (Visual)visual.Parent;
        }

        return identity;
    }

    public async void AnimateIndicatorAsync(
        Control next
    )
    {
        if (_isDisposed)
        {
            return;
        }

        var prevIndicator = _activeIndicator;
        var nextIndicator = next;

        // user clicked twice
        if (prevIndicator is null || prevIndicator.Equals(obj: nextIndicator))
        {
            return;
        }

        // Get the common ancestor as a reference point.
        var commonAncestor = prevIndicator.FindCommonVisualAncestor(target: nextIndicator);

        // likely being dragged
        if (commonAncestor is null)
        {
            return;
        }

        _activeIndicator = next;

        if (_previousAnimationOngoing)
        {
            CancelPriorAnimation();
        }

        prevIndicator.Opacity = 1;
        nextIndicator.Opacity = 0;

        // Ignore the RenderTransforms so we can get the actual positions
        var prevMatrix = GetOffsetFrom(ancestor: commonAncestor, visual: prevIndicator);
        var nextMatrix = GetOffsetFrom(ancestor: commonAncestor, visual: nextIndicator);

        var prevVector = new Point().Transform(transform: prevMatrix);
        var nextVector = new Point().Transform(transform: nextMatrix);

        var targetVector = nextVector - prevVector;
        var fromTopToBottom = targetVector.Y > 0;
        var curEasing = fromTopToBottom ? _fwdEasing : _bckEasing;
        var newDim = Math.Abs(value: NavItemsOrientation == Orientation.Vertical ? targetVector.Y : targetVector.X);
        var maxScale = newDim / (NavItemsOrientation == Orientation.Vertical
            ? nextIndicator.Bounds.Height
            : nextIndicator.Bounds.Width) + 1;

        Animation translationAnimation = new()
        {
            Easing = curEasing, Duration = _totalDuration, Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(value: 0d), Setters =
                    {
                        new Setter(property: ScaleTransform.ScaleYProperty, value: 1d)
                        , new Setter(property: TranslateTransform.XProperty, value: 0d)
                        , new Setter(property: TranslateTransform.YProperty, value: 0d)
                    }
                }
                , new KeyFrame
                {
                    Cue = new Cue(value: 0.33333d), Setters =
                    {
                        new Setter(property: ScaleTransform.ScaleYProperty, value: maxScale * 0.5d)
                    }
                }
                , new KeyFrame
                {
                    Cue = new Cue(value: 1d), Setters =
                    {
                        new Setter(property: ScaleTransform.ScaleYProperty, value: 1d)
                        , new Setter(property: TranslateTransform.XProperty, value: targetVector.X)
                        , new Setter(property: TranslateTransform.YProperty, value: targetVector.Y)
                    }
                }
            }
        };

        _previousAnimationOngoing = true;
        await translationAnimation.RunAsync(control: prevIndicator, cancellationToken: _currentAnimationCts.Token);
        _previousAnimationOngoing = false;

        prevIndicator.Opacity = 0;
        nextIndicator.Opacity = Equals(objA: _activeIndicator, objB: nextIndicator) ? 1 : 0;
    }

    public void SetActive(
        Control initial
    )
    {
        if (_activeIndicator is not null)
        {
            _activeIndicator.Opacity = 0;
        }

        _activeIndicator = initial;
        _activeIndicator.Opacity = 1;
    }
}