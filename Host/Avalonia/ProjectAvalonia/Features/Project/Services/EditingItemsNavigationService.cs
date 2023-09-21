using System;
using System.Collections.Generic;
using System.Linq;

using DynamicData;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.Services;
public class EditingItemsNavigationService : ReactiveObject
{
    private readonly SourceList<IEditingItemViewModel> editingItems;

    public EditingItemsNavigationService()
    {
        editingItems = new();
    }
    public IObservable<IChangeSet<IEditingItemViewModel>> ExportList()
    {
        return editingItems.Connect();
    }

    public void AlterItem(Func<IEditingItemViewModel, IEditingItemViewModel> editor, Func<IEditingItemViewModel, bool> itemGetter)
    {
        var item = editingItems.Items.FirstOrDefault(itemGetter);

        editingItems.Replace(item, editor(item));
    }
    public void AddItem(IEditingItemViewModel item)
    {
        editingItems.Add(item);
    }
    public void RemoveItem(IEditingItemViewModel item)
    {
        item.Dispose();

        editingItems.Remove(item);
    }

    public void RemoveItem(Func<IEditingItemViewModel, bool> getter)
    {
        var item = editingItems.Items.FirstOrDefault(getter);

        item.Dispose();

        editingItems.Remove(item);
    }
}
