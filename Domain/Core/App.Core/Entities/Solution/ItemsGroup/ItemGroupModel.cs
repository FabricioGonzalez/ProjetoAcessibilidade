namespace Core.Entities.Solution.ItemsGroup;
public class ItemGroupModel
{
    public string Name
    {
        get;
        set;
    }
    public IEnumerable<ItemModel> Items
    {
        get;
        set;
    }
}
