using System.Xml.Serialization;

using LanguageExt;
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
            if (File.Exists(path))
            {
                using var reader = new StreamReader(path);
                if (validationItemsSerealizer.Deserialize(reader) is { } result)
                {
                    return new Result<ValidationItemRoot>((ValidationItemRoot)result);
                }
            }

            return new Result<ValidationItemRoot>(new InvalidOperationException($"Erro ao Deserializar {path}"));
        }
        catch (Exception e)
        {
            return new Result<ValidationItemRoot>(e);
        }
    }

    public async Task<Result<Unit>> CreateRules(
        ValidationItemRoot rule
        , string path
    ) =>
        await Task.Run(() =>
        {
            try
            {
                using var writer =
               new StreamWriter(File.Create(path));
                validationItemsSerealizer.Serialize(textWriter: writer, o: rule);

                return new Result<Unit>();
            }
            catch (Exception e)
            {
                return new Result<Unit>(e);
            }

        });
}