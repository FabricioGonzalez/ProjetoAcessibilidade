using System.Reactive.Disposables;

using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using QuestPDF.Previewer;

using QuestPDFReport;
using QuestPDFReport.ReportSettings;

using ReactiveUI;

namespace ProjectAvalonia.PDFPreviewer.Views;
public partial class PreviewerPage : ReactiveUserControl<PreviewerViewModel>
{
    public PreviewerPage()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {


        this.WhenActivated(async (CompositeDisposable disposables) =>
        {
            var report = new StandardReport(await DataSource.GetReport(ViewModel.SolutionPath));

            this.ViewModel.Document = report;
        });
        AvaloniaXamlLoader.Load(this);
    }
}
