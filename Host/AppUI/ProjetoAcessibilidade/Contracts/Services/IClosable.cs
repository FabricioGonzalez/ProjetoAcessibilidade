using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;

using ProjetoAcessibilidade.Services;

namespace ProjetoAcessibilidade.Contracts.Services;
public interface IClosable
{
    public Window window
    {
        get;set;
    } 
    public NewItemDialogService newItemDialogService
    {
        get;set;
    }

}
