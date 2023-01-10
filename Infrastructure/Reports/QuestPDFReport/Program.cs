using Common;

using QuestPDF.Previewer;

using QuestPDFReport;
using QuestPDFReport.ReportSettings;

var path = "C:\\Users\\Ti\\Documents\\Teste\\Teste.prja";

var res = Path.Combine(Directory.GetParent(path).FullName, Constants.AppProjectItemsFolderName);

res = Constants.AppItemsTemplateFolder;

var model = await DataSource.GetReport(res, Constants.AppProjectTemplateExtension);

var Report = new StandardReport(model);

Report.ShowInPreviewer();