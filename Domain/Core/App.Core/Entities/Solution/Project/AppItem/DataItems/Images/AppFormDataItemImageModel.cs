using Core.Entities.Solution.Project.AppItem.DataItems.Images;

using ProjetoAcessibilidade.Core.Enuns;

namespace ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Images;

public class AppFormDataItemImageModel : IAppFormDataItemContract
{
    public AppFormDataItemImageModel(
        string id
        , string topic
        , AppFormDataType type = AppFormDataType.Image
    )
        : base(id: id, topic: topic, type: type)
    {
    }

    public ICollection<ImagesItem> ImagesItems
    {
        get;
        set;
    }
}