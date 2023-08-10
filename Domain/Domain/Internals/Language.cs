namespace Domain.Internals;

public class Language : ILanguage
{
    public Language(
        string code
        , string name
    )
    {
        Code = code;
        Name = name;
    }

    public string Code
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }
}