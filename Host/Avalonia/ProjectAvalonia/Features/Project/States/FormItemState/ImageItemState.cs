using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.FormItemState;
public partial class ImageItemState : ReactiveObject
{
    [AutoNotify]
    private string _id = "";

    [AutoNotify]
    private string imagePath = "";

    [AutoNotify]
    private string imageObservation = "";

}
