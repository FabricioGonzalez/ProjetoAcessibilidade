﻿using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ProjectAvalonia.Presentation.Interfaces;

namespace ProjectAvalonia.Features.Project.Views.Components;

public partial class ItemGroupView : UserControl
{
    private const string itemGroupFlag = "itemGroupContext";
    private const string itemFlag = "itemContext";

    public ItemGroupView()
    {
        InitializeComponent();

        AddHandler(routedEvent: DragDrop.DropEvent, handler: Drop);
        AddHandler(routedEvent: DragDrop.DragOverEvent, handler: DragOver);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private async void InputElement_OnPointerPressed(
        object? sender
        , PointerPressedEventArgs e
    )
    {
        var dragData = new DataObject();
        var control = sender as Control;
        var dc = control?.DataContext;

        if (dc == null)
        {
            return;
        }
        /*if(control?.DataContext is IItemGroupViewModel groupItem) {
            dragData.Set(itemGroupFlag, groupItem);
        }*/

        if (e.GetCurrentPoint(this).Properties.IsMiddleButtonPressed && control?.DataContext is IItemViewModel item)
        {
            dragData.Set(dataFormat: itemFlag, value: item);

            var result = await DragDrop.DoDragDrop(triggerEvent: e, data: dragData
                , allowedEffects: DragDropEffects.Move | DragDropEffects.Link);
        }
    }

    public static void DragOver(
        object sender
        , DragEventArgs e
    )
    {
        /*if (GetTasks(e, out var task, out var subItem)) return;*/

        /*e.DragEffects &= DragDropEffects.Move;*/
    }


    /*private static bool GetTasks(DragEventArgs e, out TaskItemViewModel? task, out TaskItemViewModel? subItem)
    {
        var control = e.Source as Control;
        task = control?.FindParentDataContext<TaskWrapperViewModel>()?.TaskItem ??
               control?.FindParentDataContext<TaskItemViewModel>();

        var sub = e.Data.Get(CustomFormat) ?? e.Data.Get(GraphControl.CustomFormat);
        subItem = sub switch
        {
            TaskWrapperViewModel taskWrapperViewModel => taskWrapperViewModel?.TaskItem,
            TaskItemViewModel taskItemViewModel => taskItemViewModel,
            _ => null
        };

        if (subItem == null || task == null)
        {
            e.DragEffects = DragDropEffects.None;
            return true;
        }

        return false;
    }*/

    public static void Drop(
        object sender
        , DragEventArgs e
    )
    {
        /*if (GetTasks(e, out var task, out var subItem)) return;*/
        var item = (IItemViewModel?)e?.Data?.Get(itemFlag);

        if (item is not null && sender is ItemGroupView itemGroup)
        {
            item.CanMoveCommand.Execute(itemGroup.DataContext as IItemGroupViewModel);
        }

        e.Handled = true;
    }
}