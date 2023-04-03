namespace Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;

public class AppOptionModel
{
    public AppOptionModel(string value, string id, bool isChecked = false)
    {
        Id = id;
        Value = value;
        IsChecked = isChecked;
    }

    public string Id
    {
        get; set;
    }
    public string Value
    {
        get; set;
    }
    public bool IsChecked
    {
        get; set;
    }
}