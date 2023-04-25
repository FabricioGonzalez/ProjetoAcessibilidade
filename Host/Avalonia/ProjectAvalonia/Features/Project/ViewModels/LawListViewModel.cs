using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class LawListViewModel : ReactiveObject, ILawListViewModel
{
    public LawListViewModel(string lawId, string lawContent)
    {
        LawId = lawId;
        LawContent = lawContent;
    }

    public string LawId
    {
        get;
    }

    public string LawContent
    {
        get;
    }
}