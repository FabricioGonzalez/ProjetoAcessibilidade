using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.EditingItemBody;

public class SolutionItemBody
    : ReactiveObject
        , ISolutionEditingBody
{
    private string _nome = "";

    public string Nome
    {
        get => _nome;
        set => this.RaiseAndSetIfChanged(ref _nome, value);
    }
}