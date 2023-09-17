using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LanguageExt;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Presentation.Interfaces.Services;

namespace ProjectAvalonia.Common.Services;
internal class FilePickerService : IFilePickerService
{
    public async Task<string> GetFolderAsync()
    {
        var data = await FileDialogHelper.GetFolderAsync();

        return data.Match(suc => suc.Path.AbsolutePath, fail => string.Empty);
    }
    public async Task<string> GetImagesAsync()
    {
        var data = await FileDialogHelper.GetImagesAsync();

        return data.Match(suc => suc.Path.AbsolutePath, fail => string.Empty);
    }
    public async Task<string> GetSolutionFilesAsync()
    {
        var data = await FileDialogHelper.GetSolutionFilesAsync();

        return data.Match(suc => suc.Path.AbsolutePath, fail => string.Empty);
    }
}
