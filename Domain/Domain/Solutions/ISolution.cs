using Domain.Solutions.LocationItem;
using Domain.Solutions.Report;

namespace Domain.Solutions;

public interface ISolution
{
    public IEnumerable<ILocationItem> Locations
    {
        get;
        set;
    }

    public IReport Report
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