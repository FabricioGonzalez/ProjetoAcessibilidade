using QuestPDF.Previewer;

using QuestPDFReport;
using QuestPDFReport.ReportSettings;

using XmlDatasource.Solution;
using XmlDatasource.Solution.DTO;

var path = "D:\\PC\\Documents\\Teste 3\\Teste 3.prja";


var solutionReader = new SolutionDatasourceImpl();

var result = solutionReader.ReadSolution(path)
    .Match(Succ: success => success, Fail: failure => new SolutionItemRoot());

var model = await DataSource.GetReport(
    result, "Legislação Vigente: NBR 9.050/15, NBR 16.537/16, Decreto Nº 5296 de 02.12.2004 e Lei Federal 13.146/16");

var report = new StandardReport(model, false);

await report.ShowInPreviewerAsync();