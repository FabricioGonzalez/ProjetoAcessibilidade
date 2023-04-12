namespace ProjectWinUI.Src.Navigation.Contracts;

public interface INavigationAware
{
    void OnNavigatedTo(
        object parameter
    );

    void OnNavigatedFrom();
}