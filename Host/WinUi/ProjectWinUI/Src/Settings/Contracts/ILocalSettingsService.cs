using System.Threading.Tasks;

namespace ProjectWinUI.Src.Settings.Contracts;

public interface ILocalSettingsService
{
    Task<T> ReadSettingAsync<T>(
        string key
    );

    Task SaveSettingAsync<T>(
        string key
        , T value
    );
}