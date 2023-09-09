using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Project.AppInstaller.ViewModels;

namespace Project.AppInstaller;

public class ViewLocator : IDataTemplate
{
    public IControl Build(
        object? data
    )
    {
        var name = data?.GetType().FullName!.Replace(oldValue: "ViewModel", newValue: "View");
        var type = Type.GetType(typeName: name!);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type: type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(
        object? data
    ) => data is ViewModelBase;
}