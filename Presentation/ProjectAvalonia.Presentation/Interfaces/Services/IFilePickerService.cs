using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;

namespace ProjectAvalonia.Presentation.Interfaces.Services;
public interface IFilePickerService
{
    Task<string> GetImagesAsync();
    Task<string> GetFolderAsync();
    Task<string> GetSolutionFilesAsync();
}
