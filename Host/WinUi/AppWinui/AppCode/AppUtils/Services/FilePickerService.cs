using AppUsecases.Contracts.Services;

using Windows.Storage.Pickers;
using WinRT.Interop;

namespace AppWinui.AppCode.AppUtils.Services;
public class FilePickerService : IFilePickerService
{
    private readonly FileOpenPicker filePicker;

    public FilePickerService()
    {
        filePicker = new FileOpenPicker();
    }
    public async Task<string?> GetFile(string[] fileFilters)
    {
        try
        {
            var hwnd = WindowNative.GetWindowHandle(App.MainWindow);

            // Associate the HWND with the file picker
            InitializeWithWindow.Initialize(filePicker, hwnd);

            // Use file picker like normal!
            if (fileFilters.Length > 0)
            {

                foreach (var item in fileFilters)
                {
                    filePicker.FileTypeFilter.Add(item);
                }
            }
            else
            {
                filePicker.FileTypeFilter.Add("*");
            }

            var file = await filePicker.PickSingleFileAsync();

            if (file != null)
                return file.Path;

            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
