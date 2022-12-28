using System.Reactive.Disposables;

using AppViewModels.System;

using Avalonia.Controls;
using Avalonia.ReactiveUI;

using Common;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Settings;
public partial class SettingsPage : ReactiveUserControl<SettingsViewModel>
{
    private TextBlock BasePath => this.FindControl<TextBlock>("basePath");
    private TextBlock AppCacheFolder => this.FindControl<TextBlock>("appCacheFolder");
    private TextBlock AppHistoryFolder => this.FindControl<TextBlock>("appHistoryFolder");
    private TextBlock AppUnclosedItemsFolder => this.FindControl<TextBlock>("appUnclosedItemsFolder");
    private TextBlock AppSettingsFolder => this.FindControl<TextBlock>("appSettingsFolder");
    private TextBlock AppUISettings => this.FindControl<TextBlock>("appUISettings");
    private TextBlock AppTemplatesFolder => this.FindControl<TextBlock>("appTemplatesFolder");
    private TextBlock AppItemsTemplateFolder => this.FindControl<TextBlock>("appItemsTemplateFolder");
    public SettingsPage()
    {
        InitializeComponent();

        ViewModel = Locator.Current.GetService<SettingsViewModel>();
        DataContext = ViewModel;

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            BasePath.Text = Constants.AppFolder;
            AppCacheFolder.Text = Constants.AppCacheFolder;
            AppHistoryFolder.Text = Constants.AppHistoryFolder;
            AppUnclosedItemsFolder.Text = Constants.AppUnclosedItemsFolder;
            AppSettingsFolder.Text = Constants.AppSettingsFolder;
            AppUISettings.Text = Constants.AppUISettings;
            AppTemplatesFolder.Text = Constants.AppTemplatesFolder;
            AppItemsTemplateFolder.Text = Constants.AppItemsTemplateFolder;
        });

    }
}
