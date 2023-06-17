using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;

using ProjectAvalonia.Features.Project.ViewModels;
using ProjectAvalonia.Presentation.Interfaces;

namespace ProjectAvalonia.Behaviors;
public class DragAndDropBehaviorExplorerItem<T> : DropHandlerBase where T : class
{
    private const string DraggingUpClassName = "dragging-up";
    private const string DraggingDownClassName = "dragging-down";
    private struct DndData
    {
        public DndData()
        {
        }
        public TreeViewItem? SrcDataGrid = null;
        public TreeViewItem DestDataGrid = null!;
        public IList<T> SrcList = null!;
        public IList<T> DestList = null!;
        public int SrcIndex = -1;

        public int DestIndex = -1;
    }
    private DndData _dnd = new();
    private bool Validate(object? sender, DragEventArgs e, object? sourceContext)
    {
        if (_dnd.SrcDataGrid is not { } srcDg ||
            sender is not TreeViewItem destDg ||
            sourceContext is not T src ||
            srcDg.Items is not IList<T> srcList ||
            destDg.Items is not IList<T> destList ||
            destDg.GetVisualAt(e.GetPosition(destDg),
              v => v.FindDescendantOfType<TreeViewItem>() is not null) is not Control
              {
                  DataContext: T dest
              } visual)
        {
            return false;
        }

        TreeViewItem cell = visual.FindDescendantOfType<TreeViewItem>()!;
        var pos = e.GetPosition(cell);

        _dnd.SrcDataGrid = srcDg;
        _dnd.DestDataGrid = destDg;
        _dnd.SrcList = srcList;
        _dnd.DestList = destList;
        _dnd.SrcIndex = srcList.IndexOf(src);
        _dnd.DestIndex = destList.IndexOf(dest);

        return true;
    }

    public override bool Validate(object? sender, DragEventArgs e, object? sourceContext,
                                  object? targetContext, object? state)
    {
        return Validate(sender, e, sourceContext);
    }

    public override bool Execute(object? sender, DragEventArgs e, object? sourceContext,
                                 object? targetContext, object? state)
    {
        if (!Validate(sender, e, sourceContext))
            return false;

        MoveItem(_dnd.SrcList, _dnd.DestList, _dnd.SrcIndex, _dnd.DestIndex);
        var treeView = (ProjectExplorerViewModel)((TreeView)_dnd.DestDataGrid.Parent).DataContext;
        _dnd.SrcDataGrid = null;
        return true;
    }

    public override void Enter(object? sender, DragEventArgs e, object? sourceContext,
                               object? targetContext)
    {
        _dnd.SrcDataGrid ??= sender as TreeViewItem;
        if (!Validate(sender, e, sourceContext))
        {
            e.DragEffects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        e.DragEffects |= DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link;
        e.Handled = true;
    }

    public override void Over(object? sender, DragEventArgs e, object? sourceContext,
                              object? targetContext)
    {
        if (!Validate(sender, e, sourceContext))
        {
            e.DragEffects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        e.DragEffects |= DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link;
        e.Handled = true;

        /*(string toAdd, string toRemove) classUpdate = _dnd.Direction switch
        {
            DragDirection.Down => (DraggingDownClassName, DraggingUpClassName),
            DragDirection.Up => (DraggingUpClassName, DraggingDownClassName),
            _ => throw new UnreachableException($"Invalid drag direction: {_dnd.Direction}")
        };
        if (_dnd.DestDataGrid.Classes.Contains(classUpdate.toAdd))
            return;

        _dnd.DestDataGrid.Classes.Remove(classUpdate.toRemove);
        _dnd.DestDataGrid.Classes.Add(classUpdate.toAdd);*/
    }

    public override void Leave(object? sender, RoutedEventArgs e)
    {
        base.Leave(sender, e);
        RemoveDraggingClass(sender as TreeViewItem);
    }

    public override void Drop(object? sender, DragEventArgs e, object? sourceContext,
                              object? targetContext)
    {
        RemoveDraggingClass(sender as TreeViewItem);
        base.Drop(sender, e, sourceContext, targetContext);
        _dnd.SrcDataGrid = null;
    }

    private static void RemoveDraggingClass(TreeViewItem? dg)
    {
        if (dg is not null && !dg.Classes.Remove(DraggingUpClassName))
            dg.Classes.Remove(DraggingDownClassName);
    }
}

