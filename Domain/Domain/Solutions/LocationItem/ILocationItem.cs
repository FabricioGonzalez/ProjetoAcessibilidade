using Domain.Shared.ValueObjects;
using Domain.Solutions.LocationItem.SectionItem;

namespace Domain.Solutions.LocationItem;

/**
 * <summary>
 *     It is a abstraction of LocationItem, needs to be implemented by some classes in order to use object substitution at
 *     runtime
 * </summary>
 */
public interface ILocationItem
{
    public ItemInfo ItemInfo
    {
        get;
        set;
    }

    public IEnumerable<ISectionItem> Sections
    {
        get;
    }

    public void AddSection(
        ISectionItem newSection
    );

    public void RemoveSection(
        ISectionItem sectionToRemove
    );
}