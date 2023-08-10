using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Enums;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class ImageContainerItemState
    : FormItemStateBase
        , IIdentifiedItem
{
    private ObservableCollection<ImageItemState> _imagesItems = new();

    private string _topic = "Imagens";

    public ImageContainerItemState(
        AppFormDataType type = AppFormDataType.Images
        , string id = ""
    )
        : base(type: type, id: id)
    {
    }

    public ObservableCollection<ImageItemState> ImagesItems
    {
        get => _imagesItems;
        set => this.RaiseAndSetIfChanged(backingField: ref _imagesItems, newValue: value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(backingField: ref _topic, newValue: value);
    }
}