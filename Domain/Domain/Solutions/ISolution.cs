using Domain.Solutions.LocationItem;

namespace Domain.Solutions;

public interface ISolution
{
    public IEnumerable<ILocationItem> Locations
    {
        get;
        set;
    }

    public void AddNewLocationItem(
        ILocationItem newItem
    );

    public void RemoveLocationItem(
        ILocationItem itemToRemove
    );
}