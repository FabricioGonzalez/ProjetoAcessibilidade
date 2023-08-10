using Domain.Solutions.ProjectItem;
using Domain.Solutions.Report;

namespace Domain.Solutions;

public interface ISolution
{
    public IEnumerable<IProjectItem> ProjectItems
    {
        get;
        set;
    }

    public IReport Report
    {
        get;
        set;
    }

    /*public void AddNewLocationItem(
        ILocationItem newItem
    );

    public void RemoveLocationItem(
        ILocationItem itemToRemove
    );*/
}