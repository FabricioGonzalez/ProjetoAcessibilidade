using ProjetoAcessibilidade.Domain.App.Models;

namespace ProjetoAcessibilidade.Domain.App.Contracts;

public interface ILanguageManager
{
    LanguageModel CurrentLanguage
    {
        get;
    }

    LanguageModel DefaultLanguage
    {
        get;
    }

    IEnumerable<LanguageModel> AllLanguages
    {
        get;
    }

    void SetLanguage(
        string languageCode
    );

    void SetLanguage(
        LanguageModel languageModel
    );
}