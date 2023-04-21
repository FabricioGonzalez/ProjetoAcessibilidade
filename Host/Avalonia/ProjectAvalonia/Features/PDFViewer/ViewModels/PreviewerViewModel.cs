using ProjectAvalonia.ViewModels.Navigation;

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
    NavigationTarget = NavigationTarget.FullScreen)]
public partial class PreviewerViewModel : RoutableViewModel
{
    // [AutoNotify] private float _currentScroll;
    //
    // [AutoNotify] private IDocument? _document;
    //
    // [AutoNotify] private float _scrollViewportSize;
    //
    // [AutoNotify] private string _solutionPath;
    // [AutoNotify] private SolutionState _solutionState;
    //
    // private bool _verticalScrollbarVisible;
    //
    // public PreviewerViewModel()
    // {
    //     SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);
    //
    //     this.WhenAnyValue(property1: vm => vm.SolutionPath)
    //         .Where(predicate: prop => !string.IsNullOrWhiteSpace(value: prop))
    //         .SubscribeAsync(onNextAsync: async prop =>
    //         {
    //             var report = new StandardReport(model: await DataSource.GetReport(
    //                 path: prop,
    //                 extension: Constants.AppProjectItemExtension));
    //
    //             Document = report;
    //         });
    //
    //     this.WhenAnyValue(property1: vm => vm.SolutionState)
    //         .WhereNotNull()
    //         .SubscribeAsync(onNextAsync: async prop =>
    //         {
    //             var report = new StandardReport(model: await DataSource.GetReport(
    //                 solutionModel: prop.ToSolutionModel(),
    //                 extension: Constants.AppProjectItemExtension));
    //
    //             Document = report;
    //         });
    //
    //     this.WhenAnyValue(property1: vm => vm.Document)
    //         .Subscribe(onNext: prop =>
    //         {
    //             UpdateDocument(document: prop);
    //         });
    //
    //     this.WhenAnyValue(property1: vm => vm.ScrollViewportSize)
    //         .Subscribe(onNext: prop =>
    //         {
    //             VerticalScrollbarVisible = prop < 1;
    //         });
    //
    //     ShowPdfCommand = ReactiveCommand.Create(execute: ShowPdf);
    //     ShowDocumentationCommand = ReactiveCommand.Create(execute: () =>
    //         OpenLink(path: "https://www.questpdf.com/documentation/api-reference.html"));
    //     PrintCommand = ReactiveCommand.Create(execute: () => OpenLink(path: "https://github.com/sponsors/QuestPDF"));
    // }
    //
    // public DocumentRenderer DocumentRenderer
    // {
    //     get;
    // } = new();
    //
    // public bool VerticalScrollbarVisible
    // {
    //     get => _verticalScrollbarVisible;
    //     private set => Dispatcher.UIThread.Post(action: () =>
    //         this.RaiseAndSetIfChanged(backingField: ref _verticalScrollbarVisible, newValue: value));
    // }
    //
    // public ReactiveCommand<Unit, Unit> ShowPdfCommand
    // {
    //     get;
    // }
    //
    // public ReactiveCommand<Unit, Unit> ShowDocumentationCommand
    // {
    //     get;
    // }
    //
    // public ReactiveCommand<Unit, Unit> SponsorProjectCommand
    // {
    //     get;
    // }
    //
    // public ReactiveCommand<Unit, Unit> PrintCommand
    // {
    //     get;
    // }
    //
    // public PreviewerViewModel SetSolutionPath(
    //     string solutionPath
    // )
    // {
    //     SolutionPath = solutionPath;
    //
    //     return this;
    // }
    //
    // protected override void OnNavigatedTo(
    //     bool isInHistory
    //     , CompositeDisposable disposables
    //     , object? Parameter = null
    // )
    // {
    //     if (Parameter is string path && !string.IsNullOrWhiteSpace(value: path))
    //     {
    //         SolutionPath = path;
    //     }
    //
    //     if (Parameter is SolutionState solution)
    //     {
    //         SolutionState = solution;
    //     }
    // }
    //
    // protected override void OnNavigatedFrom(
    //     bool isInHistory
    // )
    // {
    //     UnregisterHotReloadHandler();
    //     Document = null;
    // }
    //
    // public void UnregisterHotReloadHandler() => HotReloadManager.UpdateApplicationRequested -= InvalidateDocument;
    //
    // private void InvalidateDocument(
    //     object? sender
    //     , EventArgs e
    // ) => UpdateDocument(document: Document);
    //
    // private Task UpdateDocument(
    //     IDocument? document
    // ) => Task.Run(action: () => DocumentRenderer.UpdateDocument(document: document));
    //
    // private void ShowPdf()
    // {
    //     var filePath = Path.Combine(path1: Path.GetTempPath(), path2: $"{Guid.NewGuid():N}.pdf");
    //
    //     try
    //     {
    //         Document?.GeneratePdf(filePath: filePath);
    //     }
    //     catch (Exception exception)
    //     {
    //         new ExceptionDocument(exception: exception).GeneratePdf(filePath: filePath);
    //     }
    //
    //     OpenLink(path: filePath);
    // }
    //
    // private void OpenLink(
    //     string path
    // )
    // {
    //     var openBrowserProcess = new Process
    //     {
    //         StartInfo = new ProcessStartInfo
    //         {
    //             UseShellExecute = true, FileName = path
    //         }
    //     };
    //
    //     openBrowserProcess.Start();
    // }
}