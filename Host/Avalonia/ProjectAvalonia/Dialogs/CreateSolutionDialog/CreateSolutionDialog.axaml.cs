using System;

using AppViewModels.Dialogs;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using ReactiveUI;

namespace ProjectAvalonia.Dialogs.CreateSolutionDialog;
public partial class CreateSolutionDialog : ReactiveWindow<CreateSolutionViewModel>
{
    private Button ReturnButton => this.FindControl<Button>("CloseButton");
    private ComboBox UFList => this.FindControl<ComboBox>("UFList");

    public CreateSolutionDialog()
    {
        this.WhenAnyValue(vm => vm.DataContext)
            .WhereNotNull()
            .Subscribe(result =>
            {
                if (result is null)
                {
                    return;
                }

                ViewModel = (CreateSolutionViewModel?)result;
            });

        this.WhenActivated(disposables =>
        {
            ViewModel.CloseDialogCommand = ReactiveCommand.Create(() => Close());

            this.BindCommand(ViewModel, vm => vm.CloseDialogCommand, v => v.ReturnButton);
        });

        AvaloniaXamlLoader.Load(this);
    }
}
