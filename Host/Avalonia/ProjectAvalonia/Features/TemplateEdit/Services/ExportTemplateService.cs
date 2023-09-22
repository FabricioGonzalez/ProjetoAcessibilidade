using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

using Common;
using Common.Linq;

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

            using var newZip = ZipFile.Open(path, ZipArchiveMode.Create, Encoding.GetEncoding(866));

            Directory.GetFiles(Constants.AppValidationRulesTemplateFolder)
                .IterateOn(it =>
                {
                    newZip.CreateEntryFromFile(it, Path.Combine("Rules", Path.GetFileName(it)));
                });
            Directory.GetFiles(Constants.AppItemsTemplateFolder)
                .IterateOn(it =>
                {
                    newZip.CreateEntryFromFile(it, Path.Combine("Templates", Path.GetFileName(it)));
                });
        }
    }

}
