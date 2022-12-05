using System.Reactive.Disposables;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Project.Core.ViewModels.Main;
using ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;

using Project.Core.ViewModels.Project;

using ReactiveUI;

using Splat;
using ProjectAvalonia.Dialogs.CreateSolutionDialog;
using Project.Core.ViewModels.Dialogs;
using Avalonia.Controls;

namespace ProjectAvalonia.Views;
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{

    private MenuItem CloseApp => this.FindControl<MenuItem>("CloseAppMenuItem");
    public MainWindow()
    {
        this.WhenActivated(disposable =>
        {
            ViewModel!.ShowSolutionCreateDialog.RegisterHandler(DoShowDialogAsync);

            ViewModel.CloseAppCommand = ReactiveCommand.Create(() => Close());

            this.BindCommand(ViewModel, vm => vm.CloseAppCommand, v => v.CloseApp);

        });

        AvaloniaXamlLoader.Load(this);
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private async Task DoShowDialogAsync(InteractionContext<CreateSolutionViewModel, MainWindowViewModel?> interaction)
    {
        var dialog = new CreateSolutionDialog();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<MainWindowViewModel?>(this);
        interaction.SetOutput(result);
    }


}