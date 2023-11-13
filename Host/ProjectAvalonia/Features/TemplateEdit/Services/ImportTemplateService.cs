using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Linq;
using Ionic.Zip;
using Newtonsoft.Json.Linq;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Http;
using ProjectAvalonia.Presentation.Interfaces.Services;

namespace ProjectAvalonia.Features.TemplateEdit.Services;

public class ImportTemplateService
{
    private readonly IFilePickerService _filePickerService;
    private readonly IHttpClient _httpClient;

    public ImportTemplateService(
        IHttpClient httpClient
        , IFilePickerService filePickerService
    )
    {
        _httpClient = httpClient;
        _filePickerService = filePickerService;
    }

    public async Task ImportTemplatesFromGithub(
        string selectedFile
        , bool overwriteContent = true
    )
    {
        if (await GetTemplatesFromGithub(selectedFile) is { Length: > 0 } path)
        {
            CleanItems(overwriteContent);

            await ExtractZip(path);
        }
    }

    private void CleanItems(
        bool overwriteContent = true
    )
    {
        if (overwriteContent)
        {
            Directory.GetFiles(Constants.AppValidationRulesTemplateFolder)
                .IterateOn(rule =>
                {
                    File.Delete(rule);
                });
            Directory.GetFiles(Constants.AppItemsTemplateFolder)
                .IterateOn(rule =>
                {
                    File.Delete(rule);
                });
        }
    }

    public async Task ImportTemplatesFromFile(
        bool overwriteContent = true
    )
    {
        if (await _filePickerService.GetZipFilesAsync() is { } file)
        {
            CleanItems(overwriteContent);
            await ExtractZip(file);
        }
    }

    private async Task ExtractZip(
        string ZipPath
    )
    {
        IoHelpers.EnsureDirectoryExists(Constants.AppValidationRulesTemplateFolder);
        IoHelpers.EnsureDirectoryExists(Constants.AppItemsTemplateFolder);

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var iso = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);

        using var zip = new ZipFile(fileName: ZipPath, encoding: iso);

        foreach (var item in zip.Entries)
        {
            var rule = item.FileName.Split('/');

            var path = rule[0] switch
            {
                "Rules" => Path.Combine(path1: Constants.AppValidationRulesTemplateFolder, path2: rule[1])
                , "Templates" => Path.Combine(path1: Constants.AppItemsTemplateFolder, path2: rule[1]), _ => ""
            };
            using var sw = new FileStream(path: path
                , options: new FileStreamOptions { Mode = FileMode.Create, Access = FileAccess.Write });

            item.Extract(sw);
        }
    }

    /*  private string Decode(string name)
      {
          Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

          var iso = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);
          var utf8 = Encoding.GetEncoding("ISO-8859-1");
          var utfBytes = utf8.GetBytes(name);
          var isoBytes = Encoding.Convert(utf8, iso, utfBytes);
          return iso.GetString(isoBytes);
      }*/
    private async Task<string> GetTemplatesFromGithub(
        string selectedFile
    )
    {
        using var message = new HttpRequestMessage(method: HttpMethod.Get, requestUri: AppConstants.TemplateReleaseURL);
        message.Headers.UserAgent.Add(new ProductInfoHeaderValue(productName: "ProjetoAcessibilidade"
            , productVersion: "2.0"));
        var response = await _httpClient.SendAsync(request: message, cancellationToken: CancellationToken.None)
            .ConfigureAwait(false);
        var jsonResponse = JObject.Parse(await response.Content
            .ReadAsStringAsync(CancellationToken.None)
            .ConfigureAwait(false));

        if (jsonResponse["assets"]
                ?.Children()
                .ToList()
                .FirstOrDefault(it => it["browser_download_url"] is not null) is { } result)
        {
            using HttpClient httpClient = new();

            using var stream = await httpClient
                .GetStreamAsync(requestUri: result["browser_download_url"].ToString()
                    , cancellationToken: CancellationToken.None)
                .ConfigureAwait(false);

            var location = Path.Combine(path1: selectedFile, path2: "Items.zip");

            await CopyStreamContentToFileAsync(stream: stream, filePath: location)
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
            await stream.CopyToAsync(destination: file, cancellationToken: CancellationToken.None)
                .ConfigureAwait(false);

            // Closing the file to rename.
            file.Close();
        }

        File.Move(sourceFileName: tmpFilePath, destFileName: filePath);
    }
}

public enum ProjectTemplateImportLocation
{
    FromGithub
    , FromFile
}