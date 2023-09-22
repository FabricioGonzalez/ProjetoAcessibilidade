using System.Threading.Tasks;

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

    }

    public async Task ExportRulesAsync()
    {

    }
}
