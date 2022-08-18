using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using SystemApplication.Services.Contracts;

namespace ProjetoAcessibilidade.ViewModels;
public class PrintViewModel : ObservableRecipient
{
    private string file;
    public string File
    {

        get => file; set => SetProperty(ref file, value);
    }

    public PrintViewModel()
    {
    }
}
