using Domain.Shared.ValueObjects;

namespace Domain.Solutions.LocationItem.SectionItem.UnitItem;

public sealed class UnitItem : IUnitItem
{
    public ItemInfo ItemInfo
    {
        get;
        set;
    }
}