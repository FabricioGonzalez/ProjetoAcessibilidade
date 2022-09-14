namespace AppWinui.AppCode.AppUtils.Contracts.ViewModels;

public interface INavigationAware
{
    void OnNavigatedTo(object parameter);

    void OnNavigatedFrom();
}
