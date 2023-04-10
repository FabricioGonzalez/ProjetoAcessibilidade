using Core.Enuns;
using DynamicData.Binding;

namespace ProjectAvalonia.Features.Project.States.FormItemState;

public partial class ImageContainerItemState : FormItemStateBase
{
    [AutoNotify]
    private ObservableCollectionExtended<ImageItemState> _imagesItems = new();

    [AutoNotify]
    private string _topic = "Imagens";

    public ImageContainerItemState(
        AppFormDataType type = AppFormDataType.Image
        , string id = ""
    )
        : base(type: type, id: id)
    {
    }
}