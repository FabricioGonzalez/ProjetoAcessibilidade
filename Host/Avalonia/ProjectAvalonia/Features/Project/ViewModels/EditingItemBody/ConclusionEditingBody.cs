using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.EditingItemBody;

public class ConclusionEditingBody
    : ReactiveObject
        , IConclusionEditingBody
{
    private string _conclusionBody = "";

    public string ConclusionBody
    {
        get => _conclusionBody;
        set => this.RaiseAndSetIfChanged(ref _conclusionBody, value);
    }
}