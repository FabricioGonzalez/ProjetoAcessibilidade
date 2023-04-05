using Core.Enuns;

using DynamicData.Binding;

namespace ProjectAvalonia.Features.Project.States.FormItemState;
public partial class ImageContainerItemState : FormItemStateBase
{
    [AutoNotify]
    private string _topic = "Imagens";

    [AutoNotify]
    private ObservableCollectionExtended<ImageItemState> _imagesItems = new();

    public ImageContainerItemState(AppFormDataType type = AppFormDataType.Image,
        string id = "")
        : base(type, id)
    {

    }
}
