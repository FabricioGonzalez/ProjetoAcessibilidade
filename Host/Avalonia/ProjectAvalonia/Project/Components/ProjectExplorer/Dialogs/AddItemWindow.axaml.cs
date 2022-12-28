using System;

using AppViewModels.Dialogs;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using ReactiveUI;

namespace ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;
public partial class AddItemWindow : ReactiveWindow<AddItemViewModel>
{
    private Button ReturnButton => this.FindControl<Button>("ExitAddItemDialog");
    public AddItemWindow()
    {
        this.WhenAnyValue(vm => vm.DataContext)
            .WhereNotNull()
            .Subscribe(result =>
            {
                if (result is null)
                {
                    return;
                }

                ViewModel = (AddItemViewModel?)result;
            });

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
