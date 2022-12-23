using System;
using System.Collections.Generic;
using System.Text;

using DynamicData.Binding;

namespace Project.AppInstaller.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public ObservableCollectionExtended<string> files { get; } = new();

    public MainWindowViewModel()
    {
        var directory = Environment.CurrentDirectory;

        files.Add(directory);
    }
}
