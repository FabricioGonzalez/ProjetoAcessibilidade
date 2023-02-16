namespace ProjectAvalonia.ViewModels.Navigation;

public interface INavigatable
{
    void OnNavigatedTo(bool isInHistory);
    void OnNavigatedTo(bool isInHistory, object Parameter);

    void OnNavigatedFrom(bool isInHistory);
}
