using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

using AppUsecases.Editing.Entities;

using Project.Core.ViewModels.Extensions;

using ReactiveUI;

namespace Project.Core.ViewModels.Project;
public class AddItemViewModel : ViewModelBase
{
    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    public AddItemViewModel()
    {
        SelectItemToCreateCommand = ReactiveCommand.Create(() =>
        {
            var myString = "Selecionado";
            Debug.WriteLine(myString);

            return myString;
        });
    }

    //public Task<List<FileTemplate>> GetItems()
    //{
    //    return new();
    //} 

    public ReactiveCommand<Unit, string?> SelectItemToCreateCommand
    {
        get; set;
    }

}
