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
        var report = new StandardReport(DataSource.GetReport());



        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.ViewModel.Document = report;


        });
        AvaloniaXamlLoader.Load(this);
    }
}
