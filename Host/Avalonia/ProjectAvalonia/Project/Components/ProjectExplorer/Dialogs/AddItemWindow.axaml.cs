using System;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels.Project;

using ReactiveUI;

namespace ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;
public partial class AddItemWindow : ReactiveWindow<AddItemViewModel>
{
    private Button ReturnButton => this.FindControl<Button>("ExitAddItemDialog");
    public AddItemWindow()
    {
        this.WhenActivated(disposables =>
        {
            ViewModel.CloseDialogCommand = ReactiveCommand.Create(() => Close());

            this.BindCommand(ViewModel, vm => vm.CloseDialogCommand, v => v.ReturnButton);

            disposables(ViewModel!.SelectItemToCreateCommand.Subscribe(Close));
        }
        );

        AvaloniaXamlLoader.Load(this);
    }
}
