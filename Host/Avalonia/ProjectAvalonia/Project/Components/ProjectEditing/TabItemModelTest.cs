using AppUsecases.Project.Entities.Project;

namespace ProjectAvalonia.Project.Components.ProjectEditing;
public class TabItemModelTest
{
    public string Header
    {
        get;
    }
    public AppItemModel Content
    {
        get;
    }
    public TabItemModelTest(string header, AppItemModel content)
    {
        Header = header;
        Content = content;
    }
}
