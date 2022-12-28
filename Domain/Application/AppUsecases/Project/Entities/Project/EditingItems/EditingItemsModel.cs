
namespace AppUsecases.Project.Entities.Project.EditingItems;
public class EditingItemsModel
{
    public string Header
    {
        get;
    }
    public AppItemModel Content
    {
        get;
    }
    public EditingItemsModel(string header, AppItemModel content)
    {
        Header = header;
        Content = content;
    }
}
