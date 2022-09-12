using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using SystemApplication.Services.Contracts;
using SystemApplication.Services.UIOutputs;

using Windows.Storage;

namespace Infrastructure.WindowsStorageRepository;

public class GetAppLocalRepository : IAppLocalRepository
{
    string path;

    Uri htmlUri;
    public GetAppLocalRepository(string path)
    {
        this.path = path;
    }
    public List<FileTemplates> getProjectLocalPath()
    {
        var files = Directory.GetFiles(Path.Combine(path, "Tables"));

        List<FileTemplates> filesList = new List<FileTemplates>();

        foreach (var item in files)
        {
            var splitedItem = item.Split("\\");
            filesList.Add(new()
            {
                Name = (splitedItem.GetValue(splitedItem.Length - 1) as string).Split(".")[0],
                Path = item
            });
        }
        return filesList;
    }

    private void GenerateCheckbox(StringBuilder builder, FormDataItemCheckboxModel checkboxModel)
    {
        builder.AppendLine("<tr class=\"header\">")
            .AppendLine($"<td colspan=\"100%\">{checkboxModel.Topic}</td>")
            .AppendLine("</tr>")
            .AppendLine("<tr class=\"body\">");

        foreach (var child in checkboxModel.Children)
        {
            builder.AppendLine("<tr>")
                .AppendLine("<td class=\"topico\">")
                .AppendLine($"{child.Topic}")
                .AppendLine("</td>");

            foreach (var childOptions in child.Options)
            {
                builder.AppendLine("<td>")
                    .AppendLine($"<label>{childOptions.Value}</label>")
                    .AppendLine($"<input class=\"checkbox\" checked=\"{childOptions.IsChecked}\" type=\"checkbox\" />")
                    .AppendLine("</td>");
            }
            builder.AppendLine("</tr>");
        }

        builder.AppendLine("</tr>");
    }
    private void GenerateText(StringBuilder builder, FormDataItemTextModel textModel)
    {
        builder.AppendLine("<tr class=\"body\">")
               .AppendLine($"<td colspan=\"25%\">{textModel.Topic}</td>")
        .AppendLine("<td colspan=\"50%\">")
                .AppendLine($"{textModel.TextData}")
                .AppendLine("</td>")
        .AppendLine("<td colspan=\"25%\">")
                .AppendLine($"{textModel.MeasurementUnit}")
                .AppendLine("</td>");

        builder.AppendLine("</tr>");
    }
    private void GenerateObservation(StringBuilder builder, FormDataItemObservationModel observationModel)
    {
        builder.AppendLine("<tr class=\"header\">")
           .AppendLine($"<td colspan=\"100%\">{observationModel.Topic}</td>")
           .AppendLine("</tr>")
           .AppendLine("<tr class=\"body\">");

        builder.AppendLine("<td colspan=\"100%\">")
                .AppendLine($"{observationModel.Observation}")
                .AppendLine("</td>");

        builder.AppendLine("</tr>");
    }
    private void GenerateItem(StringBuilder builder, ItemModel item)
    {
        builder.AppendLine($"<h5 class=\"titulo\">{item.ItemName}</h5>")
            .AppendLine(" <table class=\"tabela\">")
            .AppendLine(" <tbody class=\"tabela\">");

        foreach (var formDataItem in item.FormData)
        {
            if (formDataItem is FormDataItemCheckboxModel chkbox)
            {
                GenerateCheckbox(builder, chkbox);
            }
            if (formDataItem is FormDataItemObservationModel obs)
            {
                GenerateObservation(builder, obs);
            }
            if (formDataItem is FormDataItemTextModel text)
            {
                GenerateText(builder, text);
            }
        }
        builder.AppendLine("<tbody class=\"tabela\">")
        .AppendLine("</table>");
    }
    public async void GenerateHTML(List<ItemModel> items)
    {
        var itemPath = Path.Combine(path, "HTML", "root.html");

        StorageFile file = await StorageFile.GetFileFromPathAsync(itemPath);

        StringBuilder builder = new();

        builder.AppendLine("<!DOCTYPE html>")
           .AppendLine("<html lang=\"en\">")
        .AppendLine("<head>")
        .AppendLine("<meta charset=\"UTF-8\">")
        .AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">")
        .AppendLine("<link href=\"root.css\" rel=\"stylesheet\" />")
        .AppendLine("<head>");

        builder.AppendLine("<body>");
        builder.AppendLine("<div id=\"print\">");

        foreach (var item in items)
        {
            GenerateItem(builder, item);
        }

        builder.AppendLine("</div?>");
        builder.AppendLine("</body>")
        .AppendLine("</html>");

        await file.OpenStreamForWriteAsync().ContinueWith(async (it) =>
        {
            var item = await it;

            await new StreamWriter(item).WriteAsync(builder.ToString());
        });

        htmlUri = new(itemPath);
    }

    public Uri HTMLLinkGenerator()
    {
        return htmlUri;
    }
}
