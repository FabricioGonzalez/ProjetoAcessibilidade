using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProjetoAcessibilidade.Contracts.Services;

using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;

namespace ProjetoAcessibilidade.Services;
public class FileSelectorService : IFileSelectorService
{
    private readonly FileOpenPicker openPicker;
    private readonly FileSavePicker savePicker;
    //private readonly FolderPicker folderPicker;

    public FileSelectorService()
    {
        openPicker = new FileOpenPicker();
        savePicker = new FileSavePicker();
        //folderPicker = new FolderPicker();
    }

    public void SetWindowId<T>(T picker)
    {
        var hwnd = WindowNative.GetWindowHandle(App.MainWindow);

        // Associate the HWND with the file picker
        InitializeWithWindow.Initialize(picker, hwnd);
    }
    public async Task<StorageFile> SaveFile(string filterName, string[] fileFilters, string fileName)
    {
        try
        {
            SetWindowId(savePicker);

            // Use file picker like normal!
            if (filterName.Length > 0 && fileFilters.Length > 0)
            {

                foreach (var item in fileFilters)
                {
                    savePicker.FileTypeChoices.Add(filterName, fileFilters);
                }
                savePicker.SuggestedFileName = fileName;

                var file = await savePicker.PickSaveFileAsync();

                if (file != null)
                    return file;
            }

            return null;
        }
        catch (Exception)
        {

            throw;
        }
    }
    public async Task<StorageFile> OpenFile(string[] fileFilters)
    {
        try
        {
            SetWindowId(openPicker);

            // Use file picker like normal!
            if (fileFilters.Length > 0)
            {

                foreach (var item in fileFilters)
                {
                    openPicker.FileTypeFilter.Add(item);
                }
            }
            else
            {
                openPicker.FileTypeFilter.Add("*");
            }

            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
                return file;

            return null;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
