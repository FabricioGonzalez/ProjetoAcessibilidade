using System.Xml;

using AppUsecases.App.Contracts.Repositories;
using AppUsecases.App.Models;
using AppUsecases.Project.Entities.Project;

using Common;

namespace ProjectItemReader.InternalAppFiles;
public class WriteUserSolutionRepository : IWriteContract<ProjectSolutionModel>
{
    public async Task<ProjectSolutionModel> WriteDataAsync(ProjectSolutionModel dataToWrite, string filePathToWrite)
    {
        var path = Directory.GetParent(filePathToWrite);


        var appProject = Path.Combine(path.FullName, Path.GetFileNameWithoutExtension(filePathToWrite));

        var files = GetDirectoryItems(appProject);

        var file = files.FirstOrDefault(fileItem =>
        Path.GetFileName(fileItem)
        .Equals(Path.GetFileName(filePathToWrite)));

        StreamWriter writer = null;

        CreateDirectory(appProject);

        CreateDirectory(
           Path.Combine(path.FullName,
           Path.GetFileNameWithoutExtension(filePathToWrite),
           Constants.AppProjectItemsFolderName)
           );

        var xml = CreateSolutionStructure(new XmlDocument());
        xml = SetReportData(xml, dataToWrite.reportData);
        xml = SetItemsGroup(xml, dataToWrite.ItemGroups);

        if (file is null)
        {
            writer = new StreamWriter(
                File.Create(
                    Path.Combine(path.FullName,
                Path.GetFileNameWithoutExtension(filePathToWrite),
                Path.GetFileName(filePathToWrite))
                    ));

            xml.Save(writer);

            writer.Close();
            await writer.DisposeAsync();

            return dataToWrite;
        }
        if(file is not null)
        {
            writer = new StreamWriter(file);

            xml.Save(writer);

            writer.Close();
            await writer.DisposeAsync();

            return dataToWrite;
        }

        return null;

    }

    private XmlDocument SetItemsGroup(XmlDocument xml, List<ItemGroupModel> itemGroups)
    {
        var elements = xml.GetElementsByTagName(Constants.items_groups);
        if (elements.Count > 0 && itemGroups.Count > 0)
        {
            var itemGroupsElement = elements.Item(0);

            foreach (var item in itemGroups)
            {
                var itemGroupItemElement = xml.CreateElement(Constants.items_groupsItemGroup);

                var attribute = xml.CreateAttribute(Constants.items_groupsItemGroupAttributeName);

                attribute.Value = item.Name;
                attribute.InnerXml = item.Name;

                itemGroupItemElement.SetAttributeNode(attribute);

                foreach (var subitem in item.Items)
                {
                    var subItem = xml.CreateElement(Constants.items_groups_item_group_item);

                    var subItemId = xml.CreateElement(Constants.items_groups_item_group_item_id);
                    subItemId.InnerXml = subitem.Id.ToString();

                    var subItemName = xml.CreateElement(Constants.items_groups_item_group_item_name);
                    subItemName.InnerXml = subitem.Name;

                    var subItemTemplateName = xml.CreateElement(Constants.items_groups_item_group_item_template_name);
                    subItemTemplateName.InnerXml = subitem.TemplateName;

                    subItem.AppendChild(subItemId);
                    subItem.AppendChild(subItemName);
                    subItem.AppendChild(subItemTemplateName);

                    itemGroupItemElement.AppendChild(subItem);
                }

                itemGroupsElement.AppendChild(itemGroupItemElement);
            }
        }
        return xml;
    }

    private XmlDocument SetReportData(XmlDocument xml, ReportDataModel reportData)
    {
        var elements = xml.GetElementsByTagName(Constants.report);
        if (elements.Count > 0)
        {
            var element = elements.Item(0);

            var email = xml.CreateElement(Constants.reportItemEmail);
            email.InnerText = reportData.Email;

            var endereco = xml.CreateElement(Constants.reportItemEndereco);
            endereco.InnerXml = $"{reportData.Endereco} - {(reportData.UF ?? new UF("","")).Code}";

            var nomeEmpresa = xml.CreateElement(Constants.reportItemNomeEmpresa);
            nomeEmpresa.InnerText = reportData.NomeEmpresa;

            var responsavel = xml.CreateElement(Constants.reportItemResponsavel);
            responsavel.InnerXml = reportData.Responsavel;

            var telefone = xml.CreateElement(Constants.reportItemTelefone);
            telefone.InnerXml = reportData.Telefone;

            var data = xml.CreateElement(Constants.reportItemData);
            data.InnerXml = reportData.Data.ToString("dd/MM/yyyy");

            var logo = xml.CreateElement(Constants.reportItemLogoPath);
            logo.InnerXml = reportData.LogoPath;

            var solutionName = xml.CreateElement(Constants.reportItemSolutionName);
            solutionName.InnerXml = reportData.SolutionName;

            element.AppendChild(email);
            element.AppendChild(endereco);
            element.AppendChild(nomeEmpresa);
            element.AppendChild(responsavel);
            element.AppendChild(telefone);
            element.AppendChild(telefone);
            element.AppendChild(data);
            element.AppendChild(logo);
            element.AppendChild(solutionName);
        }

        return xml;
    }

    private XmlDocument CreateSolutionStructure(XmlDocument document)
    {
        var root = document.CreateElement(Constants.solutionRoot);
        XmlElement? report = document.CreateElement(Constants.report);
        XmlElement? itemsGroups = document.CreateElement(Constants.items_groups);


        root.AppendChild(report);
        root.AppendChild(itemsGroups);

        document.AppendChild(root);

        return document;
    }

    private string[] GetDirectoryItems(string filePath)
    {
        if (Directory.Exists(filePath))
        {
            return Directory.GetFiles(filePath);
        }
        return new string[] { };
    }

    private void CreateDirectory(string filePath)
    {
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
    }
}


