namespace Domain.Solutions.ProjectItem;

public class ProjectItem : IProjectItem
{
    public string Locale
    {
        get;
        set;
    }

    public string Group
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public string TemplateName
    {
        get;
        set;
    }
}