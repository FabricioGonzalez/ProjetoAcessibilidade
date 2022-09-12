namespace Core.Models;
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