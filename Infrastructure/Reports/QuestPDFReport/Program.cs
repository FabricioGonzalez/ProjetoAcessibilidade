using Common;
using ProjectItemReader.InternalAppFiles;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Core.Entities.Solution.ReportInfo;
using QuestPDF.Previewer;
using QuestPDFReport;
using QuestPDFReport.ReportSettings;

var path = "C:\\Users\\Ti\\Pictures\\Teste\\Teste.prja";


var solutionReader = new SolutionRepositoryImpl();

var result = (await solutionReader.ReadSolution(path))
    .Match(success => success, failure => ProjectSolutionModel.Create(
        "",
        new SolutionInfo()));

var model = await DataSource.GetReport(
    result,
    Constants.AppProjectTemplateExtension);

var report = new StandardReport(model);

await report.ShowInPreviewerAsync();