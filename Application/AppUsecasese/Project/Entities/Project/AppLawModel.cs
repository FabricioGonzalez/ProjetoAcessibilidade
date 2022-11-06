using System.Collections.Generic;

namespace AppUsecases.Project.Entities.Project;

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