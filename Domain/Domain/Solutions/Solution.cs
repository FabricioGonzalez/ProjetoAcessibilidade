using Domain.Solutions.LocationItem;

namespace Domain.Solutions;

public sealed class Solution : ISolution
{
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