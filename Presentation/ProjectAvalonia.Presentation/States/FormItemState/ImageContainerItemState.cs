using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ProjetoAcessibilidade.Core.Enuns;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class ImageContainerItemState
    : FormItemStateBase
        , IIdentifiedItem
{
    private ObservableCollection<ImageItemState> _imagesItems = new();

    private string _topic = "Imagens";

    public ImageContainerItemState(
        AppFormDataType type = AppFormDataType.Image
        , string id = ""
    )
        : base(type, id)
    {
    }

    public ObservableCollection<ImageItemState> ImagesItems
    {
        get => _imagesItems;
        set => this.RaiseAndSetIfChanged(ref _imagesItems, value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }
}