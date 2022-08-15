using System;

using Microsoft.UI.Xaml;

using ProjetoAcessibilidade.Contracts.Services;
using ProjetoAcessibilidade.Helpers;
using ProjetoAcessibilidade.Views.Dialogs;

namespace ProjetoAcessibilidade.Services;
public class NewItemDialogService
{
    private Window newItemDialog = null;

    IPageService pageService;
    public NewItemDialogService(IPageService pageService)
    {
        this.pageService = pageService;
    }
    public Window GetDialog()
    {
        if (newItemDialog is null)
            return null;

        return newItemDialog;
    }
    private void CreateDialog()
    {
        if (newItemDialog is null)
        {
            newItemDialog = new Window();
        }
        return;
    }

    private void DialogShow<VmType>(
        string Title,
        Type type)
    {
        IThemeSelectorService themeSelectorService = App.GetService<IThemeSelectorService>();

        var theme = themeSelectorService.Theme;

        var dictionary = (ResourceDictionary)Application.Current.Resources.ThemeDictionaries[theme.ToString()];

        var content = (FrameworkElement)App.GetService(type);

        if (typeof(VmType) is not null)
        {
            var vm = App.GetService(typeof(VmType));

            content.DataContext = vm;

            content.RequestedTheme = theme;
        }

        if (newItemDialog is null)
        {
            CreateDialog();
        }

        newItemDialog.Title = Title;

        newItemDialog.Content = content;

        newItemDialog.SetPresenter();
        //newItemDialog.SetWindowIcon();

        newItemDialog.Closed += NewItemDialog_Closed;

        newItemDialog.Activate();
    }

    private void NewItemDialog_Closed(object sender, WindowEventArgs args)
    {
        if (sender is Window window)
            window.Closed -= NewItemDialog_Closed;
        CloseDialog();
    }

    public void ShowDialog<TViewModel>(string title)
    {
        var vmtype = pageService.GetPageType(typeof(TViewModel).FullName);
        DialogShow<TViewModel>(title, vmtype);
    }
    public void CloseDialog()
    {
        newItemDialog = null;
    }

}
