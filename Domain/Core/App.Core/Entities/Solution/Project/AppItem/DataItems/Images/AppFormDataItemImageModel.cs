using Core.Enuns;

namespace Core.Entities.Solution.Project.AppItem.DataItems.Images;
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
    } = AppFormDataType.Image;
    public ICollection<ImagesItem> ImagesItems
    {
        get; set;
    }
}
