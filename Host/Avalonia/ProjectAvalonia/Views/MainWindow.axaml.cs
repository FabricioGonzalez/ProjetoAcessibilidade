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
using AppViewModels.Main;
using Avalonia.Controls.Primitives;
using Neumorphism.Avalonia.Styles;
using AppViewModels.Interactions.Project;
using System;

namespace ProjectAvalonia.Views;
public partial class MainWindow : ReactiveWindow<MainViewModel>
{

    private MenuItem CloseApp => this.FindControl<MenuItem>("CloseAppMenuItem");
    public MainWindow()
    {

        this.WhenActivated(disposables =>
        {
            ProjectInteractions
            .SelectedProjectPath
            .RegisterHandler(async interaction =>
            {
                
                interaction.SetOutput(interaction.Input);

            }).DisposeWith(disposables);

            ViewModel!.ShowSolutionCreateDialog.RegisterHandler(async (interaction) =>
            {
                var dialog = new CreateSolutionDialog();
                dialog.DataContext = interaction.Input;

                var result = await dialog.ShowDialog<MainViewModel?>(this);
                interaction.SetOutput(result);
            })
            .DisposeWith(disposables);

            ViewModel.CloseAppCommand = ReactiveCommand.Create(() => Close())
            .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.CloseAppCommand, v => v.CloseApp)
            .DisposeWith(disposables);

        });

        AvaloniaXamlLoader.Load(this);
#if DEBUG
        this.AttachDevTools();
#endif


    }
    private void TemplatedControl_OnTemplateApplied(object sender, TemplateAppliedEventArgs e)
    {
        SnackbarHost.Post("Welcome to\r\nNeumorphism.Avalonia demo !");
    }
    public void SwitchUITheme(object sender)
    {
        var toggleButton = sender as ToggleButton;
        if (toggleButton != null)
        {
            if (toggleButton.IsChecked.HasValue && toggleButton.IsChecked.Value)
            {
                GlobalCommand.UseNeumorphismUIDarkTheme();
                SnackbarHost.Post("Neumorphism dark theme applied !");
            }
            else
            {
                GlobalCommand.UseNeumorphismUILightTheme();
                SnackbarHost.Post("Neumorphism light theme applied !");
            }
        }
    }
}