using QuestPDF.Previewer;

using QuestPDFReport;
using QuestPDFReport.ReportSettings;

var model = DataSource.GetReport();
var Report = new StandardReport(model);

Report.ShowInPreviewer();