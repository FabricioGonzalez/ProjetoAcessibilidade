using System.Xml;

using Common;

using Core.Entities.App;
using Core.Entities.Solution;
using Core.Entities.Solution.ItemsGroup;
using Core.Entities.Solution.ReportInfo;

using Project.Application.Solution.Contracts;

namespace ProjectItemReader.publicAppFiles;

public class SolutionRepositoryImpl : ISolutionRepository
{
    public async Task CreateSolution(string solutionPath)
    {
        var path = Directory.GetParent(solutionPath);

        var appProject = Path.Combine(path.FullName, Path.GetFileNameWithoutExtension(solutionPath));

        var files = GetDirectoryItems(appProject);

        var file = files.FirstOrDefault(fileItem =>
        Path.GetFileName(fileItem)
        .Equals(Path.GetFileName(solutionPath)));

        StreamWriter writer = null;

        CreateDirectory(appProject);

        CreateDirectory(
           Path.Combine(path.FullName,
           Path.GetFileNameWithoutExtension(solutionPath),
           Constants.AppProjectItemsFolderName)
               );

        var xml = CreateSolutionStructure(new XmlDocument());
        xml = SetReportData(xml, new());
        xml = SetItemsGroup(xml, new());

        if (file is null)
        {
            writer = new StreamWriter(
                File.Create(
                    Path.Combine(path.FullName,
                Path.GetFileNameWithoutExtension(solutionPath),
                Path.GetFileName(solutionPath))
                    ));

            xml.Save(writer);

            writer.Close();
            await writer.DisposeAsync();

            return;
        }
        if (file is not null)
        {
            writer = new StreamWriter(file);

            xml.Save(writer);

            writer.Close();
            await writer.DisposeAsync();

            return;
        }
    }
    public async Task<ProjectSolutionModel> ReadSolution(string solutionPath)
    {
        if (!string.IsNullOrEmpty(solutionPath))
        {
            var xml = new XmlDocument();

            xml.Load(solutionPath);

            xml.GetElementsByTagName("solution");

            var solution = new ProjectSolutionModel()
            {
                FileName = Path.GetFileNameWithoutExtension(solutionPath),
                FilePath = solutionPath,
                ParentFolderName = Path.GetFileName(Directory.GetParent(solutionPath).FullName),
                ParentFolderPath = Directory.GetParent(solutionPath).FullName,
                SolutionReportInfo = ReadSolutionInfo(xml),
                ItemGroups = ReadProjectItems(xml)
            };

            return solution;

        }
        return null;
    }
    private SolutionInfo ReadSolutionInfo(XmlDocument document)
    {
        var elements = document.GetElementsByTagName(Constants.report);

        if (elements.Count > 0)
        {
            var solutionInfo = new SolutionInfo();

            var children = elements[0].ChildNodes
                .Cast<XmlNode>();

            solutionInfo.Data = DateTimeOffset.Parse(children.First(x => x.Name.Equals(Constants.reportItemData)).InnerXml);
            solutionInfo.Responsavel = children.First(x => x.Name.Equals(Constants.reportItemResponsavel)).InnerXml;
            solutionInfo.NomeEmpresa = children.First(x => x.Name.Equals(Constants.reportItemNomeEmpresa)).InnerXml;
            solutionInfo.LogoPath = children.First(x => x.Name.Equals(Constants.reportItemLogoPath)).InnerXml;
            solutionInfo.Telefone = children.First(x => x.Name.Equals(Constants.reportItemTelefone)).InnerXml;
            solutionInfo.Endereco = children.First(x => x.Name.Equals(Constants.reportItemEndereco)).InnerXml;
            solutionInfo.Email = children.First(x => x.Name.Equals(Constants.reportItemEmail)).InnerXml;

            return solutionInfo;
        }
        return default;
    }
    private ItemGroupModel ReadItemGroups(XmlNode listItems)
    {
        var items = new ItemGroupModel();

        items.Name = listItems.Attributes
            .Cast<XmlAttribute>()
            .First(x =>
            x.Name.Equals(Constants.items_groupsItemGroupAttributeName))
            .Value;

        foreach (XmlNode item in listItems.ChildNodes)
        {
            var result = ReadItem(listItems: item.ChildNodes.Cast<XmlNode>());

            items.Items.Add(result);
        }
        return items;
    }
    private ItemModel ReadItem(IEnumerable<XmlNode> listItems)
    {
        var item = new ItemModel();

        item.Id = listItems.First(x => x.Name.Equals(Constants.items_groups_item_id)).InnerXml;
        item.Name = listItems.First(x => x.Name.Equals(Constants.items_groups_item_name)).InnerXml;
        item.TemplateName = listItems.First(x => x.Name.Equals(Constants.items_groups_item_template_name)).InnerXml;
        item.ItemPath = listItems.First(x => x.Name.Equals(Constants.items_groups_item_item_path)).InnerXml;

        return item;
    }

    private List<ItemGroupModel> ReadProjectItems(XmlDocument document)
    {
        var elements = document.GetElementsByTagName(Constants.project_items);

        var items = new List<ItemGroupModel>();

        foreach (XmlNode result in elements[0].ChildNodes)
        {
            var itemResult = ReadItemGroups(result);

            items.Add(itemResult);
        }


        return items;
    }

    public async Task SaveSolution(string solutionPath, ProjectSolutionModel dataToWrite)
    {
        var path = Directory.GetParent(solutionPath);

        var appProject = Path.Combine(path.FullName, Path.GetFileNameWithoutExtension(solutionPath));

        var files = GetDirectoryItems(appProject);

        var file = files.FirstOrDefault(fileItem =>
        Path.GetFileName(fileItem)
        .Equals(Path.GetFileName(solutionPath)));

        StreamWriter writer = null;

        CreateDirectory(appProject);

        CreateDirectory(
           Path.Combine(path.FullName,
           Path.GetFileNameWithoutExtension(solutionPath),
           Constants.AppProjectItemsFolderName)
               );

        var xml = CreateSolutionStructure(new XmlDocument());
        xml = SetReportData(xml, dataToWrite.SolutionReportInfo);
        xml = SetItemsGroup(xml, dataToWrite.ItemGroups);

        if (file is null)
        {
            writer = new StreamWriter(
                File.Create(
                    Path.Combine(path.FullName,
                Path.GetFileNameWithoutExtension(solutionPath),
                Path.GetFileName(solutionPath))
                    ));

            xml.Save(writer);

            writer.Close();
            await writer.DisposeAsync();

            return;
        }
        if (file is not null)
        {
            writer = new StreamWriter(file);

            xml.Save(writer);

            writer.Close();
            await writer.DisposeAsync();

            return;
        }
    }

    private XmlDocument SetItemsGroup(XmlDocument xml, List<ItemGroupModel> itemGroups)
    {
        var elements = xml.GetElementsByTagName(Constants.project_items);
        if (elements.Count > 0 && itemGroups.Count > 0)
        {
            var itemGroupsElement = elements.Item(0);

            foreach (var item in itemGroups)
            {
                var itemGroupItemElement = xml.CreateElement(Constants.items_groups);

                var attribute = xml.CreateAttribute(Constants.items_groupsItemGroupAttributeName);

                attribute.Value = item.Name;
                attribute.InnerXml = item.Name;

                itemGroupItemElement.SetAttributeNode(attribute);

                foreach (var subitem in item.Items)
                {
                    var subItem = xml.CreateElement(Constants.items_groups_item);

                    var subItemId = xml.CreateElement(Constants.items_groups_item_id);
                    subItemId.InnerXml = subitem.Id.ToString();

                    var subItemName = xml.CreateElement(Constants.items_groups_item_name);
                    subItemName.InnerXml = subitem.Name;

                    var subItemItemPath = xml.CreateElement(Constants.items_groups_item_item_path);
                    subItemItemPath.InnerXml = subitem.Name;

                    var subItemTemplateName = xml.CreateElement(Constants.items_groups_item_template_name);
                    subItemTemplateName.InnerXml = subitem.TemplateName;

                    subItem.AppendChild(subItemId);
                    subItem.AppendChild(subItemName);
                    subItem.AppendChild(subItemItemPath);
                    subItem.AppendChild(subItemTemplateName);

                    itemGroupItemElement.AppendChild(subItem);
                }

                itemGroupsElement.AppendChild(itemGroupItemElement);
            }
        }
        return xml;
    }

    private XmlDocument SetReportData(XmlDocument xml, SolutionInfo reportData)
    {
        var elements = xml.GetElementsByTagName(Constants.report);
        if (elements.Count > 0)
        {
            var element = elements.Item(0);

            var email = xml.CreateElement(Constants.reportItemEmail);
            email.InnerText = reportData.Email;

            var endereco = xml.CreateElement(Constants.reportItemEndereco);
            endereco.InnerXml = $"{reportData.Endereco} - {(reportData.UF ?? new UFModel("", "")).Code}";

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

            element.AppendChild(nomeEmpresa);
            element.AppendChild(responsavel);
            element.AppendChild(email);
            element.AppendChild(endereco);
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
        var report = document.CreateElement(Constants.report);
        var itemsGroups = document.CreateElement(Constants.items_groups);

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
