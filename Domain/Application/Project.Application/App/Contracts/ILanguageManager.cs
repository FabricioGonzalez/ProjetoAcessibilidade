using Project.Domain.App.Models;

namespace Project.Domain.App.Contracts;

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