﻿using QuestPDF.Previewer;
using QuestPDFReport;
using QuestPDFReport.ReportSettings;
using XmlDatasource.Solution;
using XmlDatasource.Solution.DTO;

var path = "C:\\Users\\Ti\\Pictures\\Teste 3\\Teste 3.prja";


var solutionReader = new SolutionDatasourceImpl();

var result = solutionReader.ReadSolution(path)
    .Match(Succ: success => success, Fail: failure => new SolutionItemRoot());

var model = await DataSource.GetReport(
    result /*,
    Constants.AppProjectTemplateExtension*/);

var report = new StandardReport(model);

await report.ShowInPreviewerAsync();