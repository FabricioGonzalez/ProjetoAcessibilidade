using System.Threading.Tasks;

using AppViewModels.Contracts;

using ProjectAvalonia.Views;

namespace ProjectAvalonia.Services;
public class FileDialog : IFileDialog
{
    MainWindow window;
    public FileDialog(MainWindow mainWindow)
    {
        window = mainWindow;
    }
    public async Task<string> SaveFile()
    {
        /*  var dlg = new SaveFileDialog();
          dlg.Filters.Add(new FileDialogFilter() { Name = "Arqivos de Projeto", Extensions = { "prja" } });
          dlg.Filters.Add(new FileDialogFilter() { Name = "All Files", Extensions = { "*" } });


          var openedFile = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
          {
              AllowMultiple = false
          });
          if (openedFile.Any())
          {
              var sb = new StringBuilder();

              openedFile.First().TryGetUri(out var uri);
              try
              {
                  sb.Append(uri.UriToFilePath());
              }
              catch (Exception ex)
              {
                  string text = string.Format("Error: {0}\n", ex.Message);
                  sb.Append(text);
              }

              return sb.ToString();
          }
  */
        return "";
    }
    public async Task<string> GetFolder()
    {
        /* var folder = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions() { AllowMultiple = false });

         if (folder != null)
         {
             var sb = new StringBuilder();
             folder.First().TryGetUri(out var uri);

             try
             {
                 sb.Append(uri.UriToFilePath());
             }
             catch (Exception ex)
             {
                 string text = string.Format("Error: {0}\n", ex.Message);
                 sb.Append(text);
             }

             return sb.ToString();
         }*/

        return "";
    }
    public async Task<string> GetFile(string[] fileFilters)
    {

        /*  var openedFile = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
          {
              FileTypeFilter = fileFilters.Select(item => new FilePickerFileType(item)).ToList(),
              AllowMultiple = false
          });

          if (openedFile != null)
          {
              var sb = new StringBuilder();

              try
              {
                  openedFile[0].TryGetUri(out var uri);
                  sb.Append(uri.UriToFilePath());
              }
              catch (Exception ex)
              {
                  string text = string.Format("Error: {0}\n", ex.Message);
                  sb.Append(text);
              }

              return sb.ToString();
          }*/

        return "";
    }
}
