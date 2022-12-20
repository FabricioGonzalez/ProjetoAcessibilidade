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
using AppViewModels.Interactions.Main;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace ProjectAvalonia.Views;
public partial class MainWindow : ReactiveWindow<MainViewModel>
{

    private MenuItem CloseApp => this.FindControl<MenuItem>("CloseAppMenuItem");
    private ToggleSwitch toggleButton => this.FindControl<ToggleSwitch>("toggleSwitchTheme");
    private Grid appContainer => this.Find<Grid>("AppContainer");
    public MainWindow()
    {
        this.WhenActivated(disposables =>
        {
            checkTheme();

            ViewModel.WhenAnyValue(vm => vm.appMessage)
            .Where(value => !string.IsNullOrEmpty(value.Message))
            .SubscribeOn(RxApp.MainThreadScheduler)
            .Subscribe(value =>
            {
                FlyoutBase.ShowAttachedFlyout(appContainer);
            });


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
        checkTheme();
    }

    private void checkTheme()
    {
        if (GlobalCommand.GetCurrentTheme() == Avalonia.Themes.Fluent.FluentThemeMode.Light)
        {
            toggleButton.Content = "Dark Theme";
            toggleButton.IsChecked = true;
            GlobalCommand.UseNeumorphismUIDarkTheme();
        }
        else
        {
            toggleButton.Content = "Light Theme";
            toggleButton.IsChecked = false;
            GlobalCommand.UseNeumorphismUILightTheme();
        }
        }
}