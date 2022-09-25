using AppUsecases.Contracts.Services;

using Windows.Storage.Pickers;

using WinRT.Interop;

namespace AppWinui.AppCode.AppUtils.Services;
public class FolderPickerService : IFolderPickerService
{
   private readonly FolderPicker folderPicker;

    public FolderPickerService()
    {
        folderPicker = new FolderPicker();
    }
    public async Task<string?> GetFolder()
    {
        try
        {
            var hwnd = WindowNative.GetWindowHandle(App.MainWindow);

            // Associate the HWND with the file picker
            InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.FileTypeFilter.Add("*");
            var file = await folderPicker.PickSingleFolderAsync();

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
