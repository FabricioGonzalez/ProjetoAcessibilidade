using System;

using CommunityToolkit.Mvvm.ComponentModel;

using ProjetoAcessibilidade.Contracts.ViewModels;

namespace ProjetoAcessibilidade.ViewModels;
public class PrintViewModel : ObservableRecipient,INavigationAware
{
    private string file;
    public string File
    {

        get => file; 
        set => SetProperty(ref file, value);
    }

    Uri uri;

    public PrintViewModel()
    {
    }

    public void OnNavigatedTo(object parameter)
    {
        if(parameter is Uri uri)
        {
            this.uri = uri;
        }
    }

    public void OnNavigatedFrom()
    {
    
    }
}
