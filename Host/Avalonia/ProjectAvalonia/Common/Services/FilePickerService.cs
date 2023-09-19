using System.Threading.Tasks;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Presentation.Interfaces.Services;

namespace ProjectAvalonia.Common.Services;
internal class FilePickerService : IFilePickerService
{
    public async Task<string> GetFolderAsync()
    {
        var data = await FileDialogHelper.GetFolderAsync();

        return data.Match(suc => suc.Path.LocalPath, fail => string.Empty);
    }
    public async Task<string> GetImagesAsync()
    {
        var data = await FileDialogHelper.GetImagesAsync();

        return data.Match(suc => suc.Path.LocalPath, fail => string.Empty);
    }
    public async Task<string> GetSolutionFilesAsync()
    {
        var data = await FileDialogHelper.GetSolutionFilesAsync();

        return data.Match(suc => suc.Path.LocalPath, fail => string.Empty);
    }
}
