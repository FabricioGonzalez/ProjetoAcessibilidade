using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace ProjectAvalonia.EventHelpers;

public class EventHelper
{
    private static readonly AvaloniaProperty<Handlers> EventHelperHandlers =
        AvaloniaProperty.RegisterAttached<EventHelper, Interactive, Handlers>(name: "__EventHelperHandlers");

    public static AvaloniaProperty<ICommand> PointerPressedCommandProperty
        = AvaloniaProperty.RegisterAttached<EventHelper, Interactive, ICommand>(name: "PointerPressedCommand");


    static EventHelper()
    {
        PointerPressedCommandProperty.Changed.Subscribe(onNext: args =>
        {
            var sender = (Control)args.Sender;
            var handlers = GetHandlers(o: sender);
            if (!args.NewValue.HasValue)
            {
                handlers?.PointerPressedHandler.Dispose();
                handlers.PointerPressedHandler = null;
            }
            else if (args.OldValue == null && args.NewValue.HasValue)
            {
                handlers.PointerPressedHandler = sender.AddDisposableHandler(
                    routedEvent: InputElement.PointerPressedEvent, handler: (
                        s
                        , a
                    ) =>
                    {
                        var command = GetPointerPressedCommand(c: (Control)s);
                        command?.Execute(parameter: null);
                    });
            }
        });
    }

    private static Handlers GetHandlers(
        AvaloniaObject o
    )
    {
        var v = o.GetValue(property: EventHelperHandlers) as Handlers;
        if (v == null)
        {
            o.SetValue(property: EventHelperHandlers, value: v = new Handlers());
        }

        return v;
    }

    public static void SetPointerPressedCommand(
        Control c
        , ICommand command
    ) =>
        c.SetValue(property: PointerPressedCommandProperty, value: command);

    public static ICommand GetPointerPressedCommand(
        Control c
    ) => (ICommand)c.GetValue(property: PointerPressedCommandProperty);

    private class Handlers
    {
        public IDisposable PointerPressedHandler;
    }
}