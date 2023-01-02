using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;

using AppViewModels.Common;

using ReactiveUI;

namespace AppViewModels.PDFViewer;
public class PreviewerViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen
    {
        get; set;
    }
    public string UrlPathSegment { get; } = "PDFPreviewer";

    private ObservableCollection<PreviewPage> _pages = new();
    public ObservableCollection<PreviewPage> Pages
    {
        get => _pages;
        set => this.RaiseAndSetIfChanged(ref _pages, value);
    }

    private float _currentScroll;
    public float CurrentScroll
    {
        get => _currentScroll;
        set => this.RaiseAndSetIfChanged(ref _currentScroll, value);
    }

    private float _scrollViewportSize;
    public float ScrollViewportSize
    {
        get => _scrollViewportSize;
        set
        {
            this.RaiseAndSetIfChanged(ref _scrollViewportSize, value);
            VerticalScrollbarVisible = value < 1;
        }
    }

    private bool _verticalScrollbarVisible;
    public bool VerticalScrollbarVisible
    {
        get => _verticalScrollbarVisible;
        private set => this.RaiseAndSetIfChanged(ref _verticalScrollbarVisible, value);
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

    public PreviewerViewModel()
    {

        ShowPdfCommand = ReactiveCommand.Create(ShowPdf);
        ShowDocumentationCommand = ReactiveCommand.Create(() => OpenLink("https://www.questpdf.com/api-reference/index.html"));
        SponsorProjectCommand = ReactiveCommand.Create(() => OpenLink("https://github.com/sponsors/QuestPDF"));
    }

    private void ShowPdf()
    {
        var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.pdf");
        Helpers.Helpers.GeneratePdfFromDocumentSnapshots(filePath, Pages);

        OpenLink(filePath);
    }

    private void OpenLink(string path)
    {
        using var openBrowserProcess = new Process
        {
            StartInfo = new()
            {
                UseShellExecute = true,
                FileName = path
            }
        };

        openBrowserProcess.Start();
    }

    private void HandleUpdatePreview(ICollection<PreviewPage> pages)
    {
        var oldPages = Pages;

        Pages.Clear();
        Pages = new ObservableCollection<PreviewPage>(pages);

        foreach (var page in oldPages)
        {
            page.Picture.Dispose();
        }
    }
}
