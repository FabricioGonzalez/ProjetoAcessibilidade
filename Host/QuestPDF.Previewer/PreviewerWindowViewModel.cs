using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using Avalonia.Threading;
using ReactiveUI;

namespace QuestPDF.Previewer;

internal class PreviewerWindowViewModel : ReactiveObject
{
    private float _currentScroll;
    private ObservableCollection<PreviewPage> _pages = new();

    private float _scrollViewportSize;

    private bool _verticalScrollbarVisible;

    public PreviewerWindowViewModel()
    {
        CommunicationService.Instance.OnDocumentRefreshed += HandleUpdatePreview;

        ShowPdfCommand = ReactiveCommand.Create(execute: ShowPdf);
        ShowDocumentationCommand = ReactiveCommand.Create(execute: () =>
            OpenLink(path: "https://www.questpdf.com/api-reference/index.html"));
        SponsorProjectCommand =
            ReactiveCommand.Create(execute: () => OpenLink(path: "https://github.com/sponsors/QuestPDF"));
    }

    public ObservableCollection<PreviewPage> Pages
    {
        get => _pages;
        set => this.RaiseAndSetIfChanged(backingField: ref _pages, newValue: value);
    }

    public float CurrentScroll
    {
        get => _currentScroll;
        set => this.RaiseAndSetIfChanged(backingField: ref _currentScroll, newValue: value);
    }

    public float ScrollViewportSize
    {
        get => _scrollViewportSize;
        set
        {
            this.RaiseAndSetIfChanged(backingField: ref _scrollViewportSize, newValue: value);
            VerticalScrollbarVisible = value < 1;
        }
    }

    public bool VerticalScrollbarVisible
    {
        get => _verticalScrollbarVisible;
        private set => Dispatcher.UIThread.Post(action: () =>
            this.RaiseAndSetIfChanged(backingField: ref _verticalScrollbarVisible, newValue: value));
    }

    public ReactiveCommand<Unit, Unit> ShowPdfCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ShowDocumentationCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> SponsorProjectCommand
    {
        get;
    }

    private void ShowPdf()
    {
        var filePath = Path.Combine(path1: Path.GetTempPath(), path2: $"{Guid.NewGuid():N}.pdf");
        Helpers.GeneratePdfFromDocumentSnapshots(filePath: filePath, pages: Pages);

        OpenLink(path: filePath);
    }

    private void OpenLink(
        string path
    )
    {
        using var openBrowserProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                UseShellExecute = true, FileName = path
            }
        };

        openBrowserProcess.Start();
    }

    private void HandleUpdatePreview(
        ICollection<PreviewPage> pages
    )
    {
        var oldPages = Pages;

        Pages.Clear();
        Pages = new ObservableCollection<PreviewPage>(collection: pages);

        foreach (var page in oldPages)
        {
            page.Picture.Dispose();
        }
    }
}