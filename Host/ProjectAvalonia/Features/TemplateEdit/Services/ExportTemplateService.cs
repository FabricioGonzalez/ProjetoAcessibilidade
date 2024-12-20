using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Linq;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Presentation.Interfaces.Services;

namespace ProjectAvalonia.Features.TemplateEdit.Services;

public class ExportTemplateService
{
    private readonly IFilePickerService _filePickerService;

    public ExportTemplateService(
        IFilePickerService filePickerService
    )
    {
        _filePickerService = filePickerService;
    }

    public async Task ExportItemsAsync()
    {
        try
        {
            if (await _filePickerService.GetFolderAsync() is { } file)
            {
                var path = Path.Combine(path1: file, path2: "ItensExportados.zip");

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                var iso = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);

                using var zip = ZipFile.Open(path, ZipArchiveMode.Create);

                /*zip.AddDirectory(path);*/
                if (Directory.Exists(Constants.AppValidationRulesTemplateFolder))
                {
                    Directory.GetFiles(Constants.AppValidationRulesTemplateFolder)
                        .IterateOn(it =>
                        {
                            zip.CreateEntryFromFile(it, "Rules");
                        });
                }

                if (Directory.Exists(Constants.AppItemsTemplateFolder))
                {
                    Directory.GetFiles(Constants.AppItemsTemplateFolder)
                        .IterateOn(it =>
                        {
                            zip.CreateEntryFromFile(it, "Templates");
                        });
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }
    }
}