using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;

using ProjetoAcessibilidade.Views.Dialogs;

namespace ProjetoAcessibilidade.Services;
public class NewItemDialogService
{
    private Window newItemDialog = null;

    private void CreateDialog()
    {
        if(newItemDialog is null)
        {
            newItemDialog = new NewItemDialog();
        }
        return;
    }

    public void ShowDialog(string itemPath)
    {
        
    }
}
