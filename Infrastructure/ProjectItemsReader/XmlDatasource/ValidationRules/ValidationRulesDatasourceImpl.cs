using System.Xml.Serialization;
using LanguageExt.Common;
using XmlDatasource.ValidationRules.DTO;

namespace XmlDatasource.ValidationRules;

public sealed class ValidationRulesDatasourceImpl
{
    private readonly XmlSerializer validationItemsSerealizer = new(typeof(ValidationItemRoot));

    public async Task<Result<ValidationItemRoot>> LoadRules(
        string path
    )
    {
        try
        {
            using var reader = new StreamReader(path);
            if (validationItemsSerealizer.Deserialize(reader) is { } result)
            {
                return new Result<ValidationItemRoot>((ValidationItemRoot)result);
            }

            return new Result<ValidationItemRoot>(new InvalidOperationException($"Erro ao Deserializar {path}"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<ValidationItemRoot>(e);
        }
    }

    public async Task CreateRules(
        ValidationItemRoot rule
        , string path
    ) =>
        await Task.Run(() =>
        {
            using var writer =
                new StreamWriter(File.Create(path));
            validationItemsSerealizer.Serialize(textWriter: writer, o: rule);
        });
}