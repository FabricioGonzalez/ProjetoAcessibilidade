using System.Collections.Generic;

namespace AppWinui.AppCode.Project.States.ProjectItems;

public class AppLawModel
{
    public string LawId
    {
        get; set;
    }
    public IList<string> LawTextContent
    {
        get; set;
    }
}