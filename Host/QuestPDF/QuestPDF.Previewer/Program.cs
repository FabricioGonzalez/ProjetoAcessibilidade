using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using QuestPDF.Previewer;

var applicationPort = GetCommunicationPort();
CommunicationService.Instance.Start(port: applicationPort);

if (Application.Current?.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime desktop)
{
    desktop.MainWindow = new PreviewerWindow
    {
        DataContext = new PreviewerWindowViewModel()
    };

    desktop.MainWindow.Show();
    desktop.Start(args: Array.Empty<string>());

    return;
}

AppBuilder
    .Configure(appFactory: () => new PreviewerApp())
    .UsePlatformDetect()
    .UseReactiveUI()
    .StartWithClassicDesktopLifetime(args: Array.Empty<string>());

static int GetCommunicationPort()
{
    const int defaultApplicationPort = 12500;

    var arguments = Environment.GetCommandLineArgs();

    if (arguments.Length < 2)
    {
        return defaultApplicationPort;
    }

    return int.TryParse(s: arguments[1], result: out var port) ? port : defaultApplicationPort;
}