using Domain.Solutions.ProjectItem;
using Domain.Solutions.Report;

namespace Domain.Solutions;

public sealed class Solution : ISolution
{
    public IEnumerable<IProjectItem> ProjectItems
    {
        get;
        set;
    } = Enumerable.Empty<IProjectItem>();

    public IReport Report
    {
        get;
        set;
    }

    /*public void AddNewLocationItem(
        ILocationItem newItem
    ) =>
        Locations = Locations.Where(it => it.ItemInfo.Name != newItem.ItemInfo.Name);

    public void RemoveLocationItem(
        ILocationItem itemToRemove
    ) =>
        Locations = Locations.Where(it => it.ItemInfo.Name != itemToRemove.ItemInfo.Name);*/
}