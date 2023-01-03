using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using AppViewModels.Interactions.Project;
using AppViewModels.Main;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Neumorphism.Avalonia.Styles;

using ProjectAvalonia.Dialogs.CreateSolutionDialog;

using ReactiveUI;

namespace ProjectAvalonia.Views;
public partial class MainWindow : ReactiveWindow<MainViewModel>
{

    private MenuItem CloseApp => this.FindControl<MenuItem>("CloseAppMenuItem");
    private ToggleButton toggleButton => this.FindControl<ToggleButton>("toggleSwitchTheme");
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