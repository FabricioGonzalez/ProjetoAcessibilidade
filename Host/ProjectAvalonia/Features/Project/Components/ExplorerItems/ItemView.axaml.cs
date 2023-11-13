using System;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;

namespace ProjectAvalonia.Features.Project.Components.ExplorerItems;

public partial class ItemView : UserControl
{
    public ItemView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        var events = (Observable.FromEventPattern<PointerPressedEventArgs>(target: this, eventName: "PointerPressed")
            , Observable.FromEventPattern<HoldingRoutedEventArgs>(target: this, eventName: "Holding"));
        events.Item1.Subscribe(t =>
        {
            var dragData = new DataObject();
            var control = t.Sender as Control;
            var dc = control?.DataContext;

            if (dc == null)
            {
                return;
            }

            events.Item2.SubscribeAsync(async p =>
            {
                if (p.EventArgs.HoldingState == HoldingState.Started && control?.DataContext is IItemViewModel item)
                {
                    dragData.Set(dataFormat: "itemContext", value: item);

                    var result = await DragDrop.DoDragDrop(triggerEvent: t.EventArgs, data: dragData
                        , allowedEffects: DragDropEffects.Move | DragDropEffects.Link);
                }
            });
        });
    }
}