using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public void AddItem(IEditingItemViewModel item)
    {
        editingItems.Add(item);
    }
    public void RemoveItem(IEditingItemViewModel item)
    {
        item.Dispose();

        editingItems.Remove(item);
    }
}
