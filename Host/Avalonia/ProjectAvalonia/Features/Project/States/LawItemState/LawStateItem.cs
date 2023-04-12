using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.LawItemState;

public class LawStateItem : ReactiveObject
{
    private string lawContent;
    private string lawId;

    public string LawId
    {
        get => lawId;
        set => this.RaiseAndSetIfChanged(backingField: ref lawId, newValue: value);
    }

    public string LawContent
    {
        get => lawContent;
        set => this.RaiseAndSetIfChanged(backingField: ref lawContent, newValue: value);
    }
}