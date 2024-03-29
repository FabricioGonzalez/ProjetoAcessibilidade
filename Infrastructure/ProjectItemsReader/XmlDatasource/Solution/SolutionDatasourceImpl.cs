﻿using System.Xml.Serialization;

using Common;

using LanguageExt.Common;

using XmlDatasource.Solution.DTO;

namespace XmlDatasource.Solution;

public sealed class SolutionDatasourceImpl
{
    private readonly XmlSerializer solutionSerealizer = new(typeof(SolutionItemRoot));

    public async Task SaveSolution(
        string solutionPath
        , SolutionItemRoot dataToWrite
    ) =>
        await Task.Run(() =>
        {
            using var writer =
      new StreamWriter(File.Create(solutionPath));
            solutionSerealizer.Serialize(textWriter: writer, o: dataToWrite);
        });

    /*public async Task SyncSolution(
        Option<string> solutionPath
        , Option<ProjectSolutionModel> dataToWrite
    ) =>
        solutionPath.Map(
            path =>
            {
                var xml = CreateSolutionStructure(new XmlDocument());

                return dataToWrite.MapAsync(
                        async data =>
                        {
                            var writer = new StreamWriter(
                                path,
                                false);
                            try
                            {
                                xml = SetReportData(
                                    xml,
                                    data.SolutionReportInfo);
                                xml = SetItemsGroup(
                                    xml,
                                    data.LocationItems.ToList());
                                xml.Save(writer);

                                return true;
                            }
                            finally
                            {
                                writer.Close();
                                await writer.DisposeAsync();
                            }
                        })
                    .Match(b => b, () => false);
            });*/

    public void CreateFolders(
        string solutionPath
    )
    {
        var directory = string.Join(Path.DirectorySeparatorChar, solutionPath.Split(Path.DirectorySeparatorChar)[..^1]);

        Directory.CreateDirectory(directory);

        Directory.CreateDirectory(
            Path.Combine(path1: directory
            , path2: Constants.AppProjectItemsFolderName));

        Directory.CreateDirectory(
            Path.Combine(path1: directory
            , path2: Constants.AppValidationRulesTemplateFolderName));
    }

    public Result<SolutionItemRoot> ReadSolution(
        string solutionPath
    )
    {
        try
        {
            using var reader = new StreamReader(solutionPath);
            if (solutionSerealizer.Deserialize(reader) is { } result)
            {
                var sol = (SolutionItemRoot)result;

                sol.SolutionPath = solutionPath;

                return new Result<SolutionItemRoot>(sol);
            }

            return new Result<SolutionItemRoot>(new InvalidOperationException($"Erro ao Deserializar {solutionPath}"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<SolutionItemRoot>(e);
        }
    }
}