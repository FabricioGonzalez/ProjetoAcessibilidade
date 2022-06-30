using System.Collections.Generic;

namespace Core.Models;

public class LawModel
{
    public string LawId { get; set; }
    public IEnumerable<string> LawTextContent { get; set; }
}