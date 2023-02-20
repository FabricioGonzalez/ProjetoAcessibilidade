using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.LawItemState;
public class LawStateItem : ReactiveObject
{
    private string lawId;
    public string LawId
    {
        get => lawId;
        set => this.RaiseAndSetIfChanged(ref lawId, value);
    }

    private string lawContent;
    public string LawContent
    {
        get => lawContent;
        set => this.RaiseAndSetIfChanged(ref lawContent, value);
    }
}
