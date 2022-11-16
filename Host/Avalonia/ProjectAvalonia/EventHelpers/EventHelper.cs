using System;
using System.Windows.Input;
using Avalonia.Controls;

using Avalonia.Interactivity;

using Avalonia;

namespace ProjectAvalonia.EventHelpers;
public class EventHelper
{
    private static AvaloniaProperty<Handlers> EventHelperHandlers =
        AvaloniaProperty.RegisterAttached<EventHelper, Interactive, Handlers>("__EventHelperHandlers");

    class Handlers
    {
        public IDisposable PointerPressedHandler;
    }

    static Handlers GetHandlers(AvaloniaObject o)
    {
        var v = o.GetValue(EventHelperHandlers) as Handlers;
        if (v == null)
            o.SetValue(EventHelperHandlers, v = new Handlers());
        return v;
    }

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
                handlers.PointerPressedHandler = sender.AddDisposableHandler(Control.PointerPressedEvent, (s, a) =>
                {
                    var command = GetPointerPressedCommand((Control)s);
                    command?.Execute(null);
                });
            }

        });
    }

    public static void SetPointerPressedCommand(Control c, ICommand command) =>
        c.SetValue(PointerPressedCommandProperty, command);

    public static ICommand GetPointerPressedCommand(Control c) => (ICommand)c.GetValue(PointerPressedCommandProperty);

}
