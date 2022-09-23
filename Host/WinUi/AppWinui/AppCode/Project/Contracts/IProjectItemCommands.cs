using System.Windows.Input;

namespace AppWinui.AppCode.Project.Contracts;
public interface IProjectItemCommands
{
    ICommand OpenProjectItemCommand
    {
        get;
    }
    ICommand SaveProjectItemCommand
    {
        get;
    }
}
