using System.IO;

using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Common;

using Ionic.Zip;

using Newtonsoft.Json.Linq;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Http;
using ProjectAvalonia.Presentation.Interfaces.Services;

namespace ProjectAvalonia.Features.TemplateEdit.Services;
public class ImportTemplateService
{
    private readonly IHttpClient _httpClient;
    private readonly IFilePickerService _filePickerService;

    public ImportTemplateService(IHttpClient httpClient, IFilePickerService filePickerService)
    {
        _httpClient = httpClient;
        _filePickerService = filePickerService;
    }

    public async Task ImportTemplatesFromGithub(string selectedFile)
    {
        if (await GetTemplatesFromGithub(selectedFile) is { Length: > 0 } path)
        {
            await ExtractZip(path);
        }
    }
    public async Task ImportTemplatesFromFile()
    {
        if (await _filePickerService.GetZipFilesAsync() is { } file)
        {
            await ExtractZip(file);
        }
    }

    private async Task ExtractZip(string ZipPath)
    {
        IoHelpers.EnsureDirectoryExists(Constants.AppValidationRulesTemplateFolder);
        IoHelpers.EnsureDirectoryExists(Constants.AppItemsTemplateFolder);

        using ZipFile zip = new ZipFile(ZipPath, System.Text.Encoding.UTF8);

        foreach (var item in zip.Entries)
        {
            var rule = item.FileName.Split('/');
            if (rule[0] == "Rules")
            {
                using var sw = new FileStream(Path.Combine(Constants.AppValidationRulesTemplateFolder, $"{rule[1]}"), new FileStreamOptions() { Mode = FileMode.Create, Access = FileAccess.Write });
                item.Extract(sw);
            }
            if (rule[0] == "Templates")
            {
                using var sw = new FileStream(Path.Combine(Constants.AppItemsTemplateFolder, $"{rule[1]}"), new FileStreamOptions() { Mode = FileMode.Create, Access = FileAccess.Write });
                item.Extract(sw);
            }
        }

    }

    private async Task<string> GetTemplatesFromGithub(string selectedFile)
    {
        using var message = new HttpRequestMessage(HttpMethod.Get, AppConstants.TemplateReleaseURL);
        message.Headers.UserAgent.Add(new ProductInfoHeaderValue("ProjetoAcessibilidade"
            , "2.0"));
        var response = await _httpClient.SendAsync(message, CancellationToken.None)
            .ConfigureAwait(false);
        var jsonResponse = JObject.Parse(await response.Content
            .ReadAsStringAsync(CancellationToken.None)
            .ConfigureAwait(false));

        if (jsonResponse["assets"]
            ?.Children()
            .ToList()
            .FirstOrDefault(it => it["browser_download_url"] is { }) is { } result)
        {
            using HttpClient httpClient = new();

            using var stream = await httpClient
                      .GetStreamAsync(result["browser_download_url"].ToString(), CancellationToken.None)
                      .ConfigureAwait(false);

            var location = Path.Combine(selectedFile, "Items.zip");

            await CopyStreamContentToFileAsync(stream, location)
                .ConfigureAwait(false);

            return location;
        }
        return "";
    }
    private async Task CopyStreamContentToFileAsync(
        Stream stream
        , string filePath
    )
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var tmpFilePath = $"{filePath}.tmp";
        IoHelpers.EnsureContainingDirectoryExists(tmpFilePath);
        using (var file = File.OpenWrite(tmpFilePath))
        {
            await stream.CopyToAsync(file, CancellationToken.None)
                .ConfigureAwait(false);

            // Closing the file to rename.
            file.Close();
        }

        File.Move(tmpFilePath, filePath);
    }
}

public enum ProjectTemplateImportLocation
{
    FromGithub,
    FromFile
}
