using System;
using System.Reactive.Disposables;

using AppViewModels.Dialogs;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
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

#if DEBUG
        this.AttachDevTools(gesture: KeyGesture.Parse("Shift+F2"));
#endif
    }
}
