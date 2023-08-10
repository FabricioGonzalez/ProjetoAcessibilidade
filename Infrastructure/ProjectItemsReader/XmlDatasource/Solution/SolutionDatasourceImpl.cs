using System.Xml.Serialization;
using AppRepositories.Solution.Contracts;
using AppRepositories.Solution.Dto;
using LanguageExt.Common;
using XmlDatasource.Solution.DTO;
using XmlDatasource.Solution.Mappers;

namespace XmlDatasource.Solution;

public class SolutionDatasourceImpl : ISolutionDatasource
{
    private readonly XmlSerializer solutionSerealizer = new(typeof(SolutionItemRoot));

    public async Task SaveSolution(
        string solutionPath
        , SolutionItem dataToWrite
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

    public Result<SolutionItem> ReadSolution(
        string solutionPath
    )
    {
        try
        {
            using var reader = new StreamReader(solutionPath);
            if (solutionSerealizer.Deserialize(reader) is { } result)
            {
                var deserializedData = (SolutionItemRoot)result;

                return new Result<SolutionItem>(deserializedData.ToSolutionItem());
            }

            return new Result<SolutionItem>(new InvalidOperationException($"Erro ao Deserializar {solutionPath}"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<SolutionItem>(e);
        }
    }
}