using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Models;
using ProjetoAcessibilidade.Domain.App.Contracts;
using ProjetoAcessibilidade.Domain.App.Models;

namespace ProjectAvalonia.Common.Services;

public class LanguageManager : ILanguageManager
{
    private readonly Lazy<Dictionary<string, LanguageModel>> _availableLanguages;
    private readonly LanguagesConfiguration _configuration;

    public LanguageManager(
        LanguagesConfiguration configuration
    )
    {
        _configuration = configuration;
        _availableLanguages = new Lazy<Dictionary<string, LanguageModel>>(valueFactory: GetAvailableLanguages);

        DefaultLanguage = CreateLanguageModel(cultureInfo: CultureInfo.GetCultureInfo(name: "en"));
    }

    public LanguageModel DefaultLanguage
    {
        get;
    }

    public LanguageModel CurrentLanguage => CreateLanguageModel(cultureInfo: Thread.CurrentThread.CurrentUICulture);

    public IEnumerable<LanguageModel> AllLanguages => _availableLanguages.Value.Values;

    public void SetLanguage(
        string languageCode
    )
    {
        if (string.IsNullOrEmpty(value: languageCode))
        {
            throw new ArgumentException(message: $"{nameof(languageCode)} can't be empty.");
        }

        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(name: languageCode);
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(name: languageCode);
    }

    public void SetLanguage(
        LanguageModel languageModel
    ) => SetLanguage(languageCode: languageModel.Code);

    private Dictionary<string, LanguageModel> GetAvailableLanguages() =>
        _configuration
            .AvailableLocales
            .Select(selector: locale => CreateLanguageModel(cultureInfo: new CultureInfo(name: locale)))
            .ToDictionary(keySelector: lm => lm.Code, elementSelector: lm => lm);

    private LanguageModel CreateLanguageModel(
        CultureInfo cultureInfo
    ) =>
        cultureInfo is null
            ? DefaultLanguage
            : new LanguageModel(name: cultureInfo.EnglishName, nativeName: cultureInfo.NativeName.ToTitleCase(),
                code: cultureInfo.TwoLetterISOLanguageName);
}