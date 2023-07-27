using Domain.Shared.ValueObjects;
using Domain.Solutions.LocationItem.SectionItem.UnitItem;

namespace Domain.Solutions.LocationItem.SectionItem;

public interface ISectionItem
{
    public ItemInfo ItemInfo
    {
        get;
        set;
    }

    public IEnumerable<IUnitItem> Items
    {
        get;
    }

    public void AddUnitItem(
        IUnitItem newItem
    );

    public void RemoveUnitItem(
        IUnitItem itemToRemove
    );
}