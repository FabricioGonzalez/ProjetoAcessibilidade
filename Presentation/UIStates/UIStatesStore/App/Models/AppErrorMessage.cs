namespace UIStatesStore.App.Models;
public class AppErrorMessage
{
    private string _errorMessage = "";
    public string ErrorMessage
    {
        get => _errorMessage;
        set => _errorMessage = value;
    }

    public AppErrorMessage(string message)
    {
        ErrorMessage = message;
    }
}
