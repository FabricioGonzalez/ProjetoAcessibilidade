using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Avalonia.Threading;

using Common;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.ViewModels.Dialogs.Base;

using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

using QuestPDFReport;
using QuestPDFReport.ReportSettings;

using ReactiveUI;

using Unit = System.Reactive.Unit;

namespace ProjectAvalonia.Features.PDFViewer.ViewModels;

[NavigationMetaData(
    Title = "Printer",
    Caption = "Print Report",
    Order = 1,
    Category = "Print",
    Keywords = new[] { "Print" },
    IconName = "printer_regular",
    Searchable = false,
    NavBarPosition = NavBarPosition.None,
    NavigationTarget = NavigationTarget.DialogScreen)]
public partial class PreviewerViewModel : DialogViewModelBase
{
    public DocumentRenderer DocumentRenderer { get; } = new();

    [AutoNotify] private string _solutionPath;

    [AutoNotify] private IDocument? _document;

    [AutoNotify] private float _currentScroll;

    [AutoNotify] private float _scrollViewportSize;

    private bool _verticalScrollbarVisible;
    public bool VerticalScrollbarVisible
    {
        get => _verticalScrollbarVisible;
        private set => Dispatcher.UIThread.Post(() => this.RaiseAndSetIfChanged(ref _verticalScrollbarVisible, value));
    }

    public PreviewerViewModel SetSolutionPath(string solutionPath)
    {
        SolutionPath = solutionPath;

        return this;
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
    public ReactiveCommand<Unit, Unit> PrintCommand
    {
        get;
    }

    public PreviewerViewModel()
    {
        SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

        this.WhenAnyValue(vm => vm.SolutionPath)
            .Where(prop => !string.IsNullOrWhiteSpace(prop))
            .SubscribeAsync(async prop =>
            {
                var report = new StandardReport(await DataSource.GetReport(
                path: prop,
                extension: Constants.AppProjectItemExtension));

                Document = report;
            });

        this.WhenAnyValue(vm => vm.Document)
            .Subscribe(prop =>
            {
                UpdateDocument(prop);
            });

        this.WhenAnyValue(vm => vm.ScrollViewportSize)
            .Subscribe(prop =>
            {
                VerticalScrollbarVisible = prop < 1;
            });

        ShowPdfCommand = ReactiveCommand.Create(ShowPdf);
        ShowDocumentationCommand = ReactiveCommand.Create(() => OpenLink("https://www.questpdf.com/documentation/api-reference.html"));
        PrintCommand = ReactiveCommand.Create(() => OpenLink("https://github.com/sponsors/QuestPDF"));

    }

    protected override void OnNavigatedTo(bool isInHistory, CompositeDisposable disposables, object? Parameter = null)
    {
        if (Parameter is string path && !string.IsNullOrWhiteSpace(path))
        {
            SolutionPath = path;
        }
    }

    protected override void OnNavigatedFrom(bool isInHistory)
    {
        UnregisterHotReloadHandler();
        Document = null;
    }

    public void UnregisterHotReloadHandler()
    {
        HotReloadManager.UpdateApplicationRequested -= InvalidateDocument;
    }

    private void InvalidateDocument(object? sender, EventArgs e)
    {
        UpdateDocument(Document);
    }

    private Task UpdateDocument(IDocument? document)
    {
        return Task.Run(() => DocumentRenderer.UpdateDocument(document));
    }

    private void ShowPdf()
    {
        var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.pdf");

        try
        {
            Document?.GeneratePdf(filePath);
        }
        catch (Exception exception)
        {
            new ExceptionDocument(exception).GeneratePdf(filePath);
        }

        OpenLink(filePath);
    }
    private void OpenLink(string path)
    {
        var openBrowserProcess = new Process
        {
            StartInfo = new()
            {
                UseShellExecute = true,
                FileName = path
            }
        };

        openBrowserProcess.Start();
    }
}
