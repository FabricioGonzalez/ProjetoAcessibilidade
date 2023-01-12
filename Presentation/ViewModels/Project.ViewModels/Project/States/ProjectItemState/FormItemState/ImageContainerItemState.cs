using App.Core.Enuns;

using DynamicData.Binding;

using ReactiveUI;

namespace AppViewModels.Project.States.ProjectItemState.FormItemState;
public class ImageContainerItemState : ReactiveObject
{
    private string topic = "";
    public string Topic
    {
        get => topic;
        set => this.RaiseAndSetIfChanged(ref topic, value);
    }

    private AppFormDataType type = AppFormDataType.Image;
    public AppFormDataType Type
    {
        get => type;
        set => this.RaiseAndSetIfChanged(ref type, value);
    }

    private ObservableCollectionExtended<ImageItemState> imagesItems = new();
    public ObservableCollectionExtended<ImageItemState> ImagesItems
    {
        get => imagesItems;
        set => this.RaiseAndSetIfChanged(ref imagesItems, value);
    }
}
