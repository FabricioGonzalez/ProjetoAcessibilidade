using System.Collections.Generic;

namespace SystemApplication.Services.UIOutputs;
public class LawModel
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