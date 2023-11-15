using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace ProjectAvalonia.Common.EventHelpers;

public class EventHelper
{
    private static readonly AvaloniaProperty<Handlers> EventHelperHandlers =
        AvaloniaProperty.RegisterAttached<EventHelper, Interactive, Handlers>("__EventHelperHandlers");

    public static AvaloniaProperty<ICommand> PointerPressedCommandProperty
        = AvaloniaProperty.RegisterAttached<EventHelper, Interactive, ICommand>("PointerPressedCommand");


    static EventHelper()
    {
        PointerPressedCommandProperty.Changed.Subscribe(args =>
        {
            var sender = (Control)args.Sender;
            var handlers = GetHandlers(sender);
            if (!args.NewValue.HasValue)
            {
                handlers?.PointerPressedHandler.Dispose();
                handlers.PointerPressedHandler = null;
            }
            else if (args.OldValue == null && args.NewValue.HasValue)
            {
                handlers.PointerPressedHandler = sender.AddDisposableHandler(
                    InputElement.PointerPressedEvent, (
                        s
                        , a
                    ) =>
                    {
                        var command = GetPointerPressedCommand((Control)s);
                        command?.Execute(null);
                    });
            }
        });
    }

    private static Handlers GetHandlers(
        AvaloniaObject o
    )
    {
        var v = o.GetValue(EventHelperHandlers) as Handlers;
        if (v == null)
        {
            o.SetValue(EventHelperHandlers, v = new Handlers());
        }

        return v;
    }

    public static void SetPointerPressedCommand(
        Control c
        , ICommand command
    ) =>
        c.SetValue(PointerPressedCommandProperty, command);

    public static ICommand GetPointerPressedCommand(
        Control c
    ) => (ICommand)c.GetValue(PointerPressedCommandProperty);

    private class Handlers
    {
        public IDisposable PointerPressedHandler;
    }
}