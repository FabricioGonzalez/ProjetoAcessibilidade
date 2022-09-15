using System.Collections.Generic;

namespace AppUsecases.Entities;

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