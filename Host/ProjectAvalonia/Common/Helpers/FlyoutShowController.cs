﻿using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace ProjectAvalonia.Common.Helpers;

public class FlyoutShowController : IDisposable
{
    private readonly FlyoutBase _flyout;
    private readonly Control _parent;
    private bool _isForcedOpen;

    public FlyoutShowController(
        Control parent
        , FlyoutBase flyout
    )
    {
        _flyout = flyout;
        _parent = parent;
    }

    public void Dispose() => ((Flyout)_flyout).Closing -= RejectClose;

    public void SetIsForcedOpen(
        bool value
    )
    {
        if (_isForcedOpen == value)
        {
            return;
        }

        _isForcedOpen = value;

        if (_isForcedOpen)
        {
            ((Flyout)_flyout).Closing += RejectClose;
            _flyout.ShowAt(placementTarget: _parent);
        }
        else
        {
            ((Flyout)_flyout).Closing -= RejectClose;
            _flyout.Hide();
        }
    }

    private static void RejectClose(
        object? sender
        , CancelEventArgs e
    ) => e.Cancel = true;
}