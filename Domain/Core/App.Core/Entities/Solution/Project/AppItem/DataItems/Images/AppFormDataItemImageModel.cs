using Core.Enuns;

namespace Core.Entities.Solution.Project.AppItem.DataItems.Images;
public class AppFormDataItemImageModel : IAppFormDataItemContract
{
    public AppFormDataItemImageModel(
        string id,
        string topic,
        AppFormDataType type = AppFormDataType.Image)
        : base(id, topic, type)
    {

    }
    public ICollection<ImagesItem> ImagesItems
    {
        get; set;
    }
}
