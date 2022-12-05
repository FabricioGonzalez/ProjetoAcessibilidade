using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels.Dialogs;

using ReactiveUI;

namespace ProjectAvalonia.Dialogs.CreateSolutionDialog;
public partial class CreateSolutionDialog : ReactiveWindow<CreateSolutionViewModel>
{
    private Button ReturnButton => this.FindControl<Button>("CloseButton");

    public CreateSolutionDialog()
    {
        ViewModel = DataContext as CreateSolutionViewModel;
        

        this.WhenActivated(disposables =>
        {
            ViewModel.CloseDialogCommand = ReactiveCommand.Create(() => Close());

            this.BindCommand(ViewModel, vm => vm.CloseDialogCommand, v => v.ReturnButton);

        });

        AvaloniaXamlLoader.Load(this);
    }
}
