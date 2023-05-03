namespace ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;

public class ItemGroupModel
{
    public string Name
    {
        get;
        set;
    }

    public string ItemPath
    {
        get;
        set;
    }

    public List<ItemModel> Items
    {
        get;
        set;
    } = new();
}