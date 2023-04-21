using DynamicData;

namespace ProjectAvalonia.Presentation.States.ProjectItems;

public class ItemGroupState
{
    public string Name
    {
        get;
        set;
    } = "";

    public string ItemPath
    {
        get;
        set;
    } = "";

    public SourceCache<ItemState, string> Items
    {
        get;
    } = new(keySelector: item => item.Id);

    public IEnumerable<ItemState> ItemStates
    {
        get => Items.Items.ToList();
        set => Items.AddOrUpdate(items: value);
    }
}