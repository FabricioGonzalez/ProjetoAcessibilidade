namespace App.Core.Entities.Solution.Project.AppItem.DataItems.Images;
public class AppFormDataItemImageModel : IAppFormDataItemContract
{
    public string Topic
    {
        get;
        set;
    }
    public AppFormDataType Type
    {
        get;
        set;
    } = AppFormDataType.Images;
    public ICollection<ImagesItem> ImagesItems
    {
        get; set;
    }
}
