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

    public IEnumerable<ItemModel> Items
    {
        get;
        set;
    } = Enumerable.Empty<ItemModel>();
}

public class SolutionGroupModel
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

    public IEnumerable<ItemGroupModel> Items
    {
        get;
        set;
    } = Enumerable.Empty<ItemGroupModel>();
}