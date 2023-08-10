using Domain.Internals;

namespace Application.Internals.Contracts;

public interface ILanguageManager
{
    public ILanguage DefaultLanguage
    {
        get;
    }

    public ILanguage CurrentLanguage
    {
        get;
    }

    public IEnumerable<ILanguage> AllLanguages
    {
        get;
    }

    public void SetLanguage(
        string languageCode
    );

    public void SetLanguage(
        ILanguage languageModel
    );
}