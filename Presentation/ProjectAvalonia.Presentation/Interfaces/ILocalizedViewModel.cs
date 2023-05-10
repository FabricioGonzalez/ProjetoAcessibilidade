namespace ProjectAvalonia.Presentation.Interfaces;

public interface ILocalizedViewModel
{
    public string Title
    {
        get;
        set;
    }

    public string? LocalizedTitle
    {
        get;
        set;
    }
}