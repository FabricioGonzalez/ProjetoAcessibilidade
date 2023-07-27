using Domain.Shared.ValueObjects;
using Domain.Solutions.LocationItem.SectionItem.UnitItem;

namespace Domain.Solutions.LocationItem.SectionItem;

public sealed class SectionItem : ISectionItem
{
    public ItemInfo ItemInfo
    {
        get;
        set;
    }

    public IEnumerable<IUnitItem> Items
    {
        get;
        private set;
    } = Enumerable.Empty<IUnitItem>();

    public void AddUnitItem(
        IUnitItem newItem
    ) =>
        Items = Items.Append(newItem);

    public void RemoveUnitItem(
        IUnitItem itemToRemove
    ) =>
        Items = Items.Where(it => it.ItemInfo.Name != itemToRemove.ItemInfo.Name);
}