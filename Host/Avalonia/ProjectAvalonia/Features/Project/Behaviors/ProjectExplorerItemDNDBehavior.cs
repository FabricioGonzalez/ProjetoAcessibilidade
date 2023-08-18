using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;
using ProjectAvalonia.Features.Project.ViewModels;

namespace ProjectAvalonia.Features.Project.Behaviors;

public sealed class ProjectExplorerItemDNDBehavior : DropHandlerBase
{
    /*public override void Enter(
        object? sender
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
    ) =>
        base.Enter(sender: sender, e: e, sourceContext: sourceContext, targetContext: targetContext);

    public override void Over(
        object? sender
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
    ) =>
        base.Over(sender: sender, e: e, sourceContext: sourceContext, targetContext: targetContext);

    public override void Drop(
        object? sender
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
    ) =>
        base.Drop(sender: sender, e: e, sourceContext: sourceContext, targetContext: targetContext);

    public override void Leave(
        object? sender
        , RoutedEventArgs e
    ) =>
        base.Leave(sender: sender, e: e);*/

    private bool Validate<T>(
        TreeView treeView
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
        , bool bExecute
    )
        where T : ItemViewModel
    {
        if (sourceContext is not T sourceNode
            || targetContext is not ItemGroupViewModel vm
            || treeView.GetVisualAt(e.GetPosition(treeView)) is not Control targetControl
            || targetControl.DataContext is not ItemGroupViewModel targetNode)
        {
            return false;
        }

        var sourceParent = sourceNode.Parent;
        var targetParent = targetNode;
        var sourceNodes = sourceParent is not null ? sourceParent.Items : vm.Items;
        var targetNodes = targetParent is not null ? targetParent.Items : vm.Items;

        if (sourceNodes is not null && targetNodes is not null)
        {
            var sourceIndex = sourceNodes.IndexOf(sourceNode);
            /*var targetIndex = targetNodes.IndexOf(targetNode);*/

            if (sourceIndex < 0 /*|| targetIndex < 0*/)
            {
                return false;
            }

            switch (e.DragEffects)
            {
                case DragDropEffects.Copy:
                {
                    /*if (bExecute)
                    {
                        var clone = new ItemViewModel();
                        InsertItem(targetNodes, clone, targetIndex + 1);
                    }*/

                    return true;
                }
                case DragDropEffects.Move:
                {
                    if (bExecute)
                    {
                        if (sourceNodes == targetNodes)
                        {
                            MoveItem(items: sourceNodes, sourceIndex: sourceIndex
                                , targetIndex: targetNode.Items.Count - 1);
                        }
                        else
                        {
                            sourceNode.Parent = targetParent;

                            sourceNode.CanMoveCommand.Execute(targetParent);
                            MoveItem(sourceItems: sourceNodes, targetItems: targetNodes, sourceIndex: sourceIndex
                                , targetIndex: targetNode.Items.Count - 1);
                        }
                    }

                    return true;
                }
                case DragDropEffects.Link:
                {
                    if (bExecute)
                    {
                        if (sourceNodes == targetNodes)
                        {
                            SwapItem(items: sourceNodes, sourceIndex: sourceIndex
                                , targetIndex: targetNode.Items.Count - 1);
                        }
                        else
                        {
                            sourceNode.Parent = targetParent;
                            /*targetNode. = sourceParent;*/

                            SwapItem(sourceItems: sourceNodes, targetItems: targetNodes, sourceIndex: sourceIndex
                                , targetIndex: targetNode.Items.Count - 1);
                        }
                    }

                    return true;
                }
            }
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
        if (e.Source is Control && sender is TreeView treeView)
        {
            return Validate<ItemViewModel>(treeView: treeView, e: e, sourceContext: sourceContext
                , targetContext: targetContext, bExecute: false);
        }

        return false;
    }

    public override bool Execute(
        object? sender
        , DragEventArgs e
        , object? sourceContext
        , object? targetContext
        , object? state
    )
    {
        if (e.Source is Control && sender is TreeView treeView)
        {
            return Validate<ItemViewModel>(treeView: treeView, e: e, sourceContext: sourceContext
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