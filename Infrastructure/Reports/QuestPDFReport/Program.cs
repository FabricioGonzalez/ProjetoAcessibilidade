using Common;

using ProjectItemReader.InternalAppFiles;

using QuestPDF.Previewer;

using QuestPDFReport;
using QuestPDFReport.ReportSettings;

var path = "C:\\Users\\Ti\\Documents\\Teste\\Teste.prja";

var res = Path.Combine(Directory.GetParent(path).FullName, Constants.AppProjectItemsFolderName);

res = Constants.AppItemsTemplateFolder;

var solutionReader = new SolutionRepositoryImpl();

var model = await DataSource.GetReport(solutionModel: await solutionReader.ReadSolution(path),
    extension: Constants.AppProjectTemplateExtension);

var Report = new StandardReport(model);

Report.ShowInPreviewer();