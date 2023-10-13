using System;
using System.Linq;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactions.DragAndDrop;

using ProjectAvalonia.Features.Project.ViewModels;
using ProjectAvalonia.Presentation.Interfaces;

namespace ProjectAvalonia.Features.Project.Behaviors;

public sealed class ProjectExplorerItemDNDBehavior : DropHandlerBase
{
    /*public override void Enter(
        object? sender
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
    )
    {
        if (sourceContext is IItemViewModel sourceItem && targetContext is IItemViewModel targetItem)
        {
            Console.WriteLine("Entering source -> {0} \n target -> {1}", sourceItem.ItemPath, targetItem.ItemPath);
        }

        if (sourceContext is IItemViewModel sourceItem2 && targetContext is IItemGroupViewModel targetItem2)
        {
            Console.WriteLine("Entering source -> {0} \n target -> {1}", sourceItem2.ItemPath, targetItem2.ItemPath);
        }
    }*/

    /*public override void Over(
        object? sender
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
    )
    {
        if (sourceContext is IItemViewModel sourceItem && targetContext is IItemViewModel targetItem)
        {
            Console.WriteLine("Over source -> {0} \n target -> {1}", sourceItem.ItemPath, targetItem.ItemPath);
        }

        if (sourceContext is IItemViewModel sourceItem2 && targetContext is IItemGroupViewModel targetItem2)
        {
            Console.WriteLine("Over  source -> {0} \n target -> {1}", sourceItem2.ItemPath, targetItem2.ItemPath);
        }

    }*/

    /*public override void Drop(
        object? sender
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
    )
    {
        if (sourceContext is IItemViewModel sourceItem && targetContext is IItemViewModel targetItem)
        {
            Console.WriteLine("Droping source -> {0} \n target -> {1}", sourceItem.ItemPath, targetItem.ItemPath);
        }

        if (sourceContext is IItemViewModel sourceItem2 && targetContext is IItemGroupViewModel targetItem2)
        {
            Console.WriteLine("Droping source -> {0} \n target -> {1}", sourceItem2.ItemPath, targetItem2.ItemPath);
        }

    }*/

    /*public override void Leave(
        object? sender
        , RoutedEventArgs e
    )
    {
    }*/


    private bool Validate<T>(
        TreeView treeView
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
        , bool bExecute
    )
    {
        if (sourceContext is IItemViewModel sourceItem && targetContext is IItemViewModel targetItem)
        {
            if (bExecute && e.DragEffects == DragDropEffects.Move)
            {
                var collection = sourceItem.Parent.Items;

                SwapItem(collection, collection.IndexOf(sourceItem), collection.IndexOf(targetItem));
                sourceItem.Parent.MoveItemCommand.Execute();
            }

            /*Console.WriteLine("Droping source -> {0} \n target -> {1}", sourceItem.ItemPath, targetItem.ItemPath);*/
            return true;
        }

        /*if (sourceContext is IItemViewModel sourceItem2 && targetContext is IItemGroupViewModel targetItem2)
        {
            if (bExecute && e.DragEffects == DragDropEffects.Move)
            {
                sourceItem2.Parent.Items.Remove(sourceItem2);
                targetItem2.Items.Add(sourceItem2);

            }

            Console.WriteLine("Droping source -> {0} \n target -> {1}", sourceItem2.ItemPath, targetItem2.ItemPath);
            return true;
        }*/

        if (sourceContext is IItemGroupViewModel sourceItem3 && targetContext is IItemGroupViewModel targetItem3)
        {
            if (bExecute && e.DragEffects == DragDropEffects.Move)
            {
                var collection = sourceItem3.Parent.Items;

                SwapItem(collection, collection.IndexOf(sourceItem3), collection.IndexOf(targetItem3));

                sourceItem3.MoveItemCommand.Execute();
            }
            /*Console.WriteLine("Droping source -> {0} \n target -> {1}", sourceItem3.ItemPath, targetItem3.ItemPath);*/
            return true;
        }

        /*if (sourceContext is IItemGroupViewModel sourceItem4 && targetContext is ISolutionLocationItem targetItem4)
        {
            if (bExecute && e.DragEffects == DragDropEffects.Move)
            {
                sourceItem4.Parent.Items.Remove(sourceItem4);
                targetItem4.Items.Add(sourceItem4);

            }
            Console.WriteLine("Droping source -> {0} \n target -> {1}", sourceItem4.ItemPath, targetItem4.ItemPath);
            return true;
        }*/

        if (sourceContext is ISolutionLocationItem sourceItem5 && targetContext is ISolutionLocationItem targetItem5)
        {
            if (bExecute && e.DragEffects == DragDropEffects.Move)
            {
                var items = ((ISolutionGroupViewModel)((TreeViewItem)treeView.Items?.Last()).DataContext);

                var collection = items?.LocationItems;

                var sourceIndex = collection.IndexOf(sourceItem5);
                var targetIndex = collection.IndexOf(targetItem5);

                SwapItem(collection, sourceIndex, targetIndex);
                sourceItem5.MoveItemCommand.Execute();

                Console.WriteLine("Droping source -> {0} at {2} \n target -> {1} at {3}", sourceItem5.ItemPath, targetItem5.ItemPath, sourceIndex, targetIndex);
            }


            return true;
        }
        return false;
    }

    public override bool Validate(
        object? sender
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
        , object? state
    )
    {
        if (e.Source is Control && sender is TreeViewItem treeView && GetTreeViewFromChildren(treeView) is TreeView tree)
        {
            return Validate<ItemViewModel>(treeView: tree, e: e, sourceContext: sourceContext
           , targetContext: targetContext, bExecute: false);

        }

        return false;
    }

    public TreeView GetTreeViewFromChildren(TreeViewItem treeViewItem)
    {
        if (treeViewItem.Parent is TreeView tree)
            return tree;

        return GetTreeViewFromChildren((TreeViewItem)treeViewItem.Parent);
    }

    public override bool Execute(
        object? sender
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
        , object? state
    )
    {
        if (e.Source is Control && sender is TreeViewItem treeView && GetTreeViewFromChildren(treeView) is TreeView tree)
        {
            return Validate<ItemViewModel>(treeView: tree, e: e, sourceContext: sourceContext
                , targetContext: targetContext, bExecute: true);
        }

        return false;
    }

    /*public override void Cancel(
        object? sender
        , RoutedEventArgs e
    ) =>
        base.Cancel(sender: sender, e: e);*/
}