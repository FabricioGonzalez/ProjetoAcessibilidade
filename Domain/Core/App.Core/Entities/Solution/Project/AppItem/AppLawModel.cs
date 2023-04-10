namespace Core.Entities.Solution.Project.AppItem;

public class AppLawModel
{
    public AppLawModel(
        string lawId
        , string lawTextContent
    )
    {
        LawId = lawId;
        LawTextContent = lawTextContent;
    }

    public string LawId
    {
        get;
        set;
    }

    public string LawTextContent
    {
        get;
        set;
    }
}