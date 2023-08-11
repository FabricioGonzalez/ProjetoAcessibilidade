using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.App;

public sealed class LanguageState : ReactiveObject
{
    public LanguageState(
        string name
        , string code
    )
    {
        Name = name;
        Code = code;
    }

    public string Name
    {
        get;
        set;
    }

    public string Code
    {
        get;
        set;
    }
}