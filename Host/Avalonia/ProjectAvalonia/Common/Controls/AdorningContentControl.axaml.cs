using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using ProjectAvalonia.Common.Helpers;

namespace ProjectAvalonia.Common.Controls;

public enum RevealMode
{
    Manual
    , PointerOver
}

public class AdorningContentControl : ContentControl
{
    public static readonly StyledProperty<Control> AdornmentProperty =
        AvaloniaProperty.Register<AdorningContentControl, Control>(name: nameof(Adornment));

    public static readonly StyledProperty<RevealMode> RevealModeProperty =
        AvaloniaProperty.Register<AdorningContentControl, RevealMode>(name: nameof(RevealMode)
            , defaultValue: RevealMode.PointerOver);

    public static readonly StyledProperty<bool> IsAdornmentVisibleProperty =
        AvaloniaProperty.Register<AdorningContentControl, bool>(name: nameof(IsAdornmentVisible), defaultValue: true);

    private IDisposable? _subscription;

    public Control Adornment
    {
        get => GetValue(property: AdornmentProperty);
        set => SetValue(property: AdornmentProperty, value: value);
    }

    public RevealMode RevealMode
    {
        get => GetValue(property: RevealModeProperty);
        set => SetValue(property: RevealModeProperty, value: value);
    }

    public bool IsAdornmentVisible
    {
        get => GetValue(property: IsAdornmentVisibleProperty);
        set => SetValue(property: IsAdornmentVisibleProperty, value: value);
    }

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change
    )
    {
        base.OnPropertyChanged(change: change);

        if (change.Property == IsPointerOverProperty)
        {
            Dispatcher.UIThread.Post(action: OnPointerOverChanged);
        }
        else if (change.Property == AdornmentProperty)
        {
            var (oldValue, newValue) = change.GetOldAndNewValue<Control?>();
            InvalidateAdornmentVisible(oldValue: oldValue
                , newValue: newValue);
        }
        else if (change.Property == IsAdornmentVisibleProperty)
        {
            if (change.GetNewValue<bool>())
            {
                AdornerHelper.AddAdorner(visual: this, adorner: Adornment);
            }
            else
            {
                AdornerHelper.RemoveAdorner(visual: this, adorner: Adornment);
            }
        }
    }

    protected override void OnDetachedFromVisualTree(
        VisualTreeAttachmentEventArgs e
    )
    {
        base.OnDetachedFromVisualTree(e: e);

        _subscription?.Dispose();

        InvalidateAdornmentVisible(oldValue: null, newValue: null);
    }

    protected override void OnAttachedToVisualTree(
        VisualTreeAttachmentEventArgs e
    )
    {
        base.OnAttachedToVisualTree(e: e);

        InvalidateAdornmentVisible(oldValue: null, newValue: Adornment);

        OnPointerOverChanged();
    }

    private void InvalidateAdornmentVisible(
        Control? oldValue
        , Control? newValue
    )
    {
        _subscription?.Dispose();

        if (oldValue is not null)
        {
            AdornerHelper.RemoveAdorner(visual: this, adorner: oldValue);
        }

        if (newValue is not null)
        {
            _subscription = newValue.GetObservable(property: IsPointerOverProperty)
                .Subscribe(onNext: _ => Dispatcher.UIThread.Post(action: OnPointerOverChanged));

            if (IsAdornmentVisible)
            {
                AdornerHelper.AddAdorner(visual: this, adorner: Adornment);
            }
        }
    }

    private void OnPointerOverChanged()
    {
        if (RevealMode == RevealMode.PointerOver)
        {
            if (IsPointerOver)
            {
                IsAdornmentVisible = true;
            }
            else if (!Adornment.IsPointerOver)
            {
                IsAdornmentVisible = false;
            }
        }
    }
}