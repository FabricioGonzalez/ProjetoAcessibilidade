using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAvalonia.Project.Components.ProjectEditing;
public class TabItemModelTest
{
    public string Header
    {
        get;
    }
    public string Content
    {
        get;
    }
    public TabItemModelTest(string header, string content)
    {
        Header = header;
        Content = content;
    }
}
