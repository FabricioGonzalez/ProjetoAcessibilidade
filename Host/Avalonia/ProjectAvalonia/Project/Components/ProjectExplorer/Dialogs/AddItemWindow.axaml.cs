using System;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels.Project;

using ReactiveUI;

namespace ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;
public partial class AddItemWindow : ReactiveWindow<AddItemViewModel>
{
    private Button ReturnButton;
    public AddItemWindow()
    {
/*        ReturnButton = this.FindControl<Button>("ExitAddItemDialog");

        ReturnButton.Command = ReactiveCommand.Create(() =>
        {
            Close();
        });*/

        this.WhenActivated(disposables => 
        disposables(ViewModel!.SelectItemToCreateCommand.Subscribe(Close)));

        AvaloniaXamlLoader.Load(this);
    }
}
