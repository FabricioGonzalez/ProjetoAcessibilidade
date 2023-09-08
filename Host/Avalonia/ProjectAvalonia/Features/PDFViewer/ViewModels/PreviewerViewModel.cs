using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;
using Avalonia.Threading;

using Common;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Navigation;

using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

using QuestPDFReport;
using QuestPDFReport.ReportSettings;

using ReactiveUI;

using SkiaSharp;

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
    NavigationTarget = NavigationTarget.FullScreen)]
public partial class PreviewerViewModel : RoutableViewModel
{
    private ObservableCollection<Printer> _availablePrinters = new();
    [AutoNotify] private float _currentScroll;

    [AutoNotify] private IDocument? _document;

    [AutoNotify] private float _scrollViewportSize;

    [AutoNotify] private Printer? _selectedPrinter;
    [AutoNotify] private SolutionState _solutionModel;

    [AutoNotify] private string _solutionPath;

    private bool _verticalScrollbarVisible;

    private int PageCounter;

    public PreviewerViewModel()
    {
        SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

        AvailablePrinters.Clear();

        foreach (var item in PrinterSettings.InstalledPrinters)
        {
            AvailablePrinters.Add(new Printer
            {
                Name = item.ToString()
            });
        }


        this.WhenAnyValue(vm => vm.SolutionPath)
            .Where(prop => !string.IsNullOrWhiteSpace(prop))
            .SubscribeAsync(async prop =>
            {
                var report = new StandardReport(await DataSource.GetReport(
                    path: prop,
                    extension: Constants.AppProjectItemExtension));

                Document = report;
            });

        this.WhenAnyValue(vm => vm.SolutionModel)
            .WhereNotNull()
            .SubscribeAsync(async prop =>
            {
                var report = new StandardReport(await DataSource.GetReport(
                    prop.ToSolutionItemRoot(), ServicesConfig.UiConfig.DefaultLawContent /*,
                    Constants.AppProjectItemExtension*/));

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
        ShowDocumentationCommand = ReactiveCommand.Create(() =>
            OpenLink("https://www.questpdf.com/documentation/api-reference.html"));
        PrintCommand = ReactiveCommand.Create(() => OpenLink("https://github.com/sponsors/QuestPDF"));
    }

    public ObservableCollection<Printer> AvailablePrinters
    {
        get => _availablePrinters;
        set => this.RaiseAndSetIfChanged(backingField: ref _availablePrinters, newValue: value);
    }

    public DocumentRenderer DocumentRenderer
    {
        get;
    } = new();

    public override MenuViewModel? ToolBar => null;

    public bool VerticalScrollbarVisible
    {
        get => _verticalScrollbarVisible;
        private set => Dispatcher.UIThread.Post(() =>
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

    public ReactiveCommand<Unit, Unit> PrintCommand
    {
        get;
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    }

    public ReactiveCommand<Unit, Unit> PrintDocumentCommand => ReactiveCommand.CreateRunInBackground(execute: () =>
    {
        PrintDocument();
    } /*, canExecute: this.WhenAnyValue(vm => vm.SelectedPrinter).Select(it => it is not null)*/);

    private async Task PrintDocument()
    {
        try
        {
            /*if (SelectedPrinter is { } printer)
            {
                var document = new PrintDocument();

                document.PrinterSettings = new PrinterSettings
                {
                    PrinterName = printer.Name
                };

                document.BeginPrint += pd_BeginPrint;
                document.PrintPage += pd_PrintPage;

                document.Print();

                document.Dispose();

                document.BeginPrint -= pd_BeginPrint;
                document.PrintPage -= pd_PrintPage;
            }*/

            (await FileDialogHelper.SaveFile(suggestedName: $"{SolutionModel.FileName} - relatorio"
                , fileTypes: new List<FilePickerFileType>
                {
                    FilePickerFileTypes.Pdf
                }
            )).IfSucc(succ =>
            {
                Document?.GeneratePdf(succ.Path.LocalPath);
            });
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void pd_BeginPrint(
        object sender
        , PrintEventArgs ev
    ) =>
        PageCounter = 0;

    private void pd_PrintPage(
        object sender
        , PrintPageEventArgs ev
    )
    {
        try
        {
            var image = DocumentRenderer.Pages[PageCounter];

            var imageConverted = SKImage.FromPicture(picture: image.Picture
                , dimensions: new SKSizeI(width: Convert.ToInt32(image.Size.Width)
                    , height: Convert.ToInt32(image.Size.Height)));

            var encoded = imageConverted.Encode();
            // get a stream over the encoded data
            var stream = encoded.AsStream();

            var bitmap = Image.FromStream(stream);

            ev.Graphics.DrawImage(image: bitmap, x: 0, y: 0);
            PageCounter++;
            ev.HasMorePages = PageCounter != DocumentRenderer.Pages.Count;
        }
        catch (Exception exp)
        {
            Logger.LogError(exp);
            /*MessageBox.Show("An error occurred whiling printing: " + exp.Message);*/
        }
    }

    public PreviewerViewModel SetSolutionPath(
        string solutionPath
    )
    {
        SolutionPath = solutionPath;

        return this;
    }

    protected override void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
        , object? Parameter = null
    )
    {
        if (Parameter is string path && !string.IsNullOrWhiteSpace(path))
        {
            SolutionPath = path;
        }

        if (Parameter is SolutionState solution)
        {
            SolutionModel = null;

            SolutionModel = solution;
        }
    }

    protected override void OnNavigatedFrom(
        bool isInHistory
    )
    {
        UnregisterHotReloadHandler();
        Document = null;
        SolutionModel = null;
    }

    public void UnregisterHotReloadHandler() => HotReloadManager.UpdateApplicationRequested -= InvalidateDocument;

    private void InvalidateDocument(
        object? sender
        , EventArgs e
    ) => UpdateDocument(Document);

    private Task UpdateDocument(
        IDocument? document
    ) => Task.Run(() => DocumentRenderer.UpdateDocument(document));

    private void ShowPdf()
    {
        var filePath = Path.Combine(path1: Path.GetTempPath(), path2: $"{Guid.NewGuid():N}.pdf");

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

    private void OpenLink(
        string path
    )
    {
        var openBrowserProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = path
            }
        };

        openBrowserProcess.Start();
    }
}