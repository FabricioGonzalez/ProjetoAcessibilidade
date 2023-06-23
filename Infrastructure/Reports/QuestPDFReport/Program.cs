using Common;
using Common.Optional;

using ProjectItemReader.InternalAppFiles;

using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Core.Entities.Solution.ReportInfo;

using QuestPDF.Previewer;

using QuestPDFReport;
using QuestPDFReport.ReportSettings;

var path = "C:\\Users\\Ti\\Pictures\\Teste\\Teste.prja";


var solutionReader = new SolutionRepositoryImpl();

var result = (await solutionReader.ReadSolution(solutionPath: path.ToOption()))
    .Match(success => success, failure => ProjectSolutionModel.Create(
        solutionPath: "",
        reportInfo: new SolutionInfo()));

var model = await DataSource.GetReport(
        solutionModel: result,
        extension: Constants.AppProjectTemplateExtension);

var report = new StandardReport(model: model);

await report.ShowInPreviewerAsync();