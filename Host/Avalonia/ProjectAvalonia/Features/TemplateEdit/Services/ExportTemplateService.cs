using System.IO;
using System.Threading.Tasks;

using Common;
using Common.Linq;

using Ionic.Zip;

using ProjectAvalonia.Presentation.Interfaces.Services;

namespace ProjectAvalonia.Features.TemplateEdit.Services;
public class ExportTemplateService
{
    private readonly IFilePickerService _filePickerService;

    public ExportTemplateService(IFilePickerService filePickerService)
    {
        _filePickerService = filePickerService;
    }

    public async Task ExportItemsAsync()
    {

        if (await _filePickerService.GetFolderAsync() is { } file)
        {
            var path = Path.Combine(file, "ItensExportados.zip");

            if (!File.Exists(path))
            {
                File.Delete(path);
            }
            using ZipFile zip = new ZipFile();

            zip.AddDirectory(path);

            zip.AddDirectory(path);

            Directory.GetFiles(Constants.AppValidationRulesTemplateFolder)
                .IterateOn(it =>
                {
                    zip.AddFile(it, Path.Combine("Rules", Path.GetFileName(it)));
                });
            Directory.GetFiles(Constants.AppItemsTemplateFolder)
                .IterateOn(it =>
                {
                    zip.AddFile(it, Path.Combine("Templates", Path.GetFileName(it)));
                });
            zip.Save(path);
        }
    }

}
