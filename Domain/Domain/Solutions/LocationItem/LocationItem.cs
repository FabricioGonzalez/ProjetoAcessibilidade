using Domain.Shared.ValueObjects;
using Domain.Solutions.LocationItem.SectionItem;

namespace Domain.Solutions.LocationItem;

/**
 * <summary>
 *     This class Stands for location Item, Grouping the Section Items
 * </summary>
 */
public sealed class LocationItem : ILocationItem
{
    public ItemInfo ItemInfo
    {
        get;
        set;
    }

    public IEnumerable<ISectionItem> Sections
    {
        get;
        private set;
    } = Enumerable.Empty<ISectionItem>();

    public void AddSection(
        ISectionItem newSection
    ) => Sections = Sections.Append(newSection);

    public void RemoveSection(
        ISectionItem sectionToRemove
    ) =>
        Sections = Sections.Where(it => it.ItemInfo.Name != sectionToRemove.ItemInfo.Name);
}