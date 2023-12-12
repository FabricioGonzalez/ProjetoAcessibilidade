using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;
using Avalonia.Threading;

using Common;

using LanguageExt;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Features.Project.Mappings;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Navigation;

using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

using QuestPDFReport;
using QuestPDFReport.Models;
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
    Keywords = ["Print"],
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
    [AutoNotify] private SolutionState? _solutionModel;

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
                    extension: Constants.AppProjectItemExtension), ServicesConfig.UiConfig.ImageStrecthing);

                Document = report;
            });

        this.WhenAnyValue(vm => vm.ScrollViewportSize)
            .Subscribe(prop =>
            {
                VerticalScrollbarVisible = prop < 1;
            });

        /*ShowPdfCommand = ReactiveCommand.CreateFromTask(ShowPdf);*/
        ShowDocumentationCommand = ReactiveCommand.Create(() =>
            OpenLink("https://www.questpdf.com/documentation/api-reference.html"));
        PrintCommand = ReactiveCommand.Create(() => OpenLink("https://github.com/sponsors/QuestPDF"));


        LoadPdfCommand = ReactiveCommand.CreateRunInBackground<SolutionState, Task>(async report =>
        {
            var obs = Observable.Return(true);

            obs.ToProperty(this, x => x.IsBusy, out _isBusy).Dispose();

            SolutionModel = null;

            SolutionModel = report;

            var data = await DataSource
                .GetReport(solutionModel: report.ToSolutionItemRoot(),
                standardLaw: ServicesConfig.UiConfig.DefaultLawContent);

            var result = new StandardReport(data,
                strechImages: ServicesConfig.UiConfig.ImageStrecthing);

            Document = result;

            await UpdateDocument(Document);
            obs = Observable.Return(false);
            obs.ToProperty(this, x => x.IsBusy, out _isBusy).Dispose();
        });
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

    public ReactiveCommand<Unit, Task> ShowPdfCommand
    {
        get;
    }

    public ReactiveCommand<SolutionState, Task> LoadPdfCommand
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

    public ReactiveCommand<Unit, Task> PrintDocumentCommand => ReactiveCommand.CreateRunInBackground(PrintDocument);

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
            )).IfSucc(async succ =>
            {
                var obs = Observable.Return(true);

                obs.ToProperty(this, x => x.IsBusy, out _isBusy).Dispose();

                var filePath = succ.Path.LocalPath;

                await Task.Run(() =>
                {
                    try
                    {
                        Document?.GeneratePdf(filePath);

                    }
                    catch (Exception exception)
                    {
                        new ExceptionDocument(exception).GeneratePdf(filePath);
                    }

                })
                        .ContinueWith(t =>
                        {
                            OpenLink(filePath);
                            obs = Observable.Return(false);

                            obs.ToProperty(this, x => x.IsBusy, out _isBusy).Dispose();
                        });
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

            var bitmap = System.Drawing.Image.FromStream(stream);

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

    protected override void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
        , object? Parameter = null
    )
    {
        if (Parameter is SolutionState solution)
        {
            LoadPdfCommand.Execute(solution).Subscribe();
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

    private async void InvalidateDocument(
        object? sender
        , EventArgs e
    ) => await UpdateDocument(Document);

    private Task UpdateDocument(
        IDocument? document
    ) => Task.Run(() => DocumentRenderer.UpdateDocument(document));

    private async Task ShowPdf()
    {
        var obs = Observable.Return(true);

        obs.ToProperty(this, x => x.IsBusy, out _isBusy).Dispose();

        var filePath = Path.Combine(path1: Path.GetTempPath(), path2: $"{Guid.NewGuid():N}.pdf");
        await Task.Run(() =>
        {
            try
            {
                Document?.GeneratePdf(filePath);

            }
            catch (Exception exception)
            {
                new ExceptionDocument(exception).GeneratePdf(filePath);
            }

        })
                .ContinueWith(t =>
                {
                    OpenLink(filePath);
                    obs = Observable.Return(false);

                    obs.ToProperty(this, x => x.IsBusy, out _isBusy).Dispose();
                });
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