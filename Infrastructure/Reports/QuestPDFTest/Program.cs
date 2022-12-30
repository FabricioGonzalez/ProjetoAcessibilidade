using QuestPDF.Previewer;

using QuestPDFTest;
using QuestPDFTest.ReportSettings;

var model = DataSource.GetReport();
var Report = new StandardReport(model);

Report.ShowInPreviewer();