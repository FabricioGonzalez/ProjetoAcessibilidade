using Domain.Shared.ValueObjects;

namespace Domain.Solutions.LocationItem.SectionItem.UnitItem;

public interface IUnitItem
{
    public ItemInfo ItemInfo
    {
        get;
        set;
    }
}