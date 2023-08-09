using Domain.Solutions.LocationItem;
using Domain.Solutions.Report;

namespace Domain.Solutions;

public sealed class Solution : ISolution
{
    public IReport Report
    {
        get;
        set;
    }

    public IEnumerable<ILocationItem> Locations
    {
        get;
        set;
    } = Enumerable.Empty<ILocationItem>();

    public void AddNewLocationItem(
        ILocationItem newItem
    ) =>
        Locations = Locations.Where(it => it.ItemInfo.Name != newItem.ItemInfo.Name);

    public void RemoveLocationItem(
        ILocationItem itemToRemove
    ) =>
        Locations = Locations.Where(it => it.ItemInfo.Name != itemToRemove.ItemInfo.Name);
}