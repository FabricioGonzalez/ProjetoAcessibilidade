using Core.Contracts;

namespace Core.Models;

public class FormDataItemModel
{
    public IFormDataItemContract Item
    {
        get; set;
    }

    public FormDataItemModel(IFormDataItemContract Item)
    {
        this.Item = Item;
    }
}
