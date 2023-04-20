using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using Common;
using Core.Entities.App;
using Core.Entities.Solution;
using Core.Entities.Solution.ItemsGroup;
using Core.Entities.Solution.ReportInfo;
using Project.Domain.Solution.Contracts;
using ProjectItemReader.InternalAppFiles.DTO;

namespace ProjectItemReader.InternalAppFiles;

public class SolutionRepositoryImpl : ISolutionRepository
{
    public async Task<ProjectSolutionModel> ReadSolution(
        string solutionPath
    )
    {
        if (!string.IsNullOrEmpty(value: solutionPath))
        {
            var xml = new XmlDocument();

            xml.Load(filename: solutionPath);

            xml.GetElementsByTagName(name: "solution");

            var solution = new ProjectSolutionModel
            {
                FileName = Path.GetFileNameWithoutExtension(path: solutionPath), FilePath = solutionPath,
                ParentFolderName = Path.GetFileName(path: Directory.GetParent(path: solutionPath).FullName),
                ParentFolderPath = Directory.GetParent(path: solutionPath).FullName,
                SolutionReportInfo = ReadSolutionInfo(document: xml),
                ItemGroups = ReadProjectItems(document: xml, solutionPath: solutionPath)
            };

            return solution;
        }

        return null;
    }

    public async Task SaveSolution(
        string solutionPath
        , ProjectSolutionModel dataToWrite
    )
    {
        /*ar path = Directory.GetParent(solutionPath);

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
        }*/
        var serializer = new XmlSerializer(type: typeof(SolutionItemRoot));

        using var writer = new StreamWriter(stream: File.Create(path: solutionPath));

        serializer.Serialize(textWriter: writer, o: dataToWrite.ToItemRoot());
    }

    public async Task SyncSolution(
        string solutionPath
        , ProjectSolutionModel dataToWrite
    )
    {
        var xml = CreateSolutionStructure(document: new XmlDocument());
        xml = SetReportData(xml: xml, reportData: dataToWrite.SolutionReportInfo);
        xml = SetItemsGroup(xml: xml, itemGroups: dataToWrite.ItemGroups);

        var writer = new StreamWriter(
            path: solutionPath, append: false);

        xml.Save(writer: writer);

        writer.Close();
        await writer.DisposeAsync();
    }

    private SolutionInfo ReadSolutionInfo(
        XmlDocument document
    )
    {
        var elements = document.GetElementsByTagName(name: Constants.report);

        if (elements.Count > 0)
        {
            var solutionInfo = new SolutionInfo();

            var children = elements[i: 0].ChildNodes
                .Cast<XmlNode>();

            var data = children.First(predicate: x => x.Name.Equals(value: Constants.reportItemData)).InnerXml;

            solutionInfo.Data = DateTime.TryParseExact(
                s: data,
                format: "dd/MM/yyyy",
                provider: CultureInfo.InvariantCulture,
                style: DateTimeStyles.None,
                result: out var date)
                ? date
                : DateTimeOffset.Now;

            solutionInfo.Responsavel =
                children.First(predicate: x => x.Name.Equals(value: Constants.reportItemResponsavel)).InnerXml;

            solutionInfo.NomeEmpresa =
                children.First(predicate: x => x.Name.Equals(value: Constants.reportItemNomeEmpresa)).InnerXml;

            solutionInfo.LogoPath = children.First(predicate: x => x.Name.Equals(value: Constants.reportItemLogoPath))
                .InnerXml;

            solutionInfo.Telefone = children.First(predicate: x => x.Name.Equals(value: Constants.reportItemTelefone))
                .InnerXml;

            solutionInfo.Endereco = children.First(predicate: x => x.Name.Equals(value: Constants.reportItemEndereco))
                .InnerXml;

            solutionInfo.Email = children.First(predicate: x => x.Name.Equals(value: Constants.reportItemEmail))
                .InnerXml;

            return solutionInfo;
        }

        return default;
    }

    private ItemGroupModel ReadItemGroups(
        XmlNode listItems
        , string solutionPath
    )
    {
        var items = new ItemGroupModel();

        items.Name = listItems?.Attributes
            ?.Cast<XmlAttribute>()
            .First(predicate: x =>
                x.Name.Equals(value: Constants.items_groupsItemGroupAttributeName))
            .Value;

        items.ItemPath = Path.Combine(
            path1: string.Join(separator: Path.DirectorySeparatorChar
                , value: solutionPath.Split(separator: Path.DirectorySeparatorChar)[..^1]),
            path2: Constants.AppProjectItemsFolderName,
            path3: items?.Name);

        foreach (XmlNode item in listItems?.ChildNodes)
        {
            var result = ReadItem(listItems: item.ChildNodes.Cast<XmlNode>());

            items.Items.Add(item: result);
        }

        return items;
    }

    private ItemModel ReadItem(
        IEnumerable<XmlNode> listItems
    )
    {
        var item = new ItemModel();

        item.Id = listItems.First(predicate: x => x.Name.Equals(value: Constants.items_groups_item_id)).InnerXml;
        item.Name = listItems.First(predicate: x => x.Name.Equals(value: Constants.items_groups_item_name)).InnerXml;
        item.TemplateName = listItems
            .First(predicate: x => x.Name.Equals(value: Constants.items_groups_item_template_name)).InnerXml;
        item.ItemPath = listItems.First(predicate: x => x.Name.Equals(value: Constants.items_groups_item_item_path))
            .InnerXml;

        return item;
    }

    private List<ItemGroupModel> ReadProjectItems(
        XmlDocument document
        , string solutionPath
    )
    {
        var elements = document.GetElementsByTagName(name: Constants.project_items);

        var items = new List<ItemGroupModel>();

        foreach (XmlNode result in elements[i: 0]?.ChildNodes)
        {
            var itemResult = ReadItemGroups(listItems: result, solutionPath: solutionPath);

            items.Add(item: itemResult);
        }


        return items;
    }

    private XmlDocument SetItemsGroup(
        XmlDocument xml
        , List<ItemGroupModel> itemGroups
    )
    {
        var elements = xml.GetElementsByTagName(name: Constants.project_items);
        if (elements.Count > 0 && itemGroups.Count > 0)
        {
            var itemGroupsElement = elements.Item(index: 0);

            foreach (var item in itemGroups)
            {
                var itemGroupItemElement = xml.CreateElement(name: Constants.items_groups);

                var attribute = xml.CreateAttribute(name: Constants.items_groupsItemGroupAttributeName);

                attribute.Value = item.Name;
                attribute.InnerXml = item.Name;

                itemGroupItemElement.SetAttributeNode(newAttr: attribute);

                foreach (var subitem in item.Items)
                {
                    var subItem = xml.CreateElement(name: Constants.items_groups_item);

                    var subItemId = xml.CreateElement(name: Constants.items_groups_item_id);
                    subItemId.InnerXml = subitem.Id;

                    var subItemName = xml.CreateElement(name: Constants.items_groups_item_name);
                    subItemName.InnerXml = subitem.Name;

                    var subItemItemPath = xml.CreateElement(name: Constants.items_groups_item_item_path);
                    subItemItemPath.InnerXml = subitem.ItemPath;

                    var subItemTemplateName = xml.CreateElement(name: Constants.items_groups_item_template_name);
                    subItemTemplateName.InnerXml = subitem.TemplateName;

                    subItem.AppendChild(newChild: subItemId);
                    subItem.AppendChild(newChild: subItemName);
                    subItem.AppendChild(newChild: subItemItemPath);
                    subItem.AppendChild(newChild: subItemTemplateName);

                    itemGroupItemElement.AppendChild(newChild: subItem);
                }

                itemGroupsElement?.AppendChild(newChild: itemGroupItemElement);
            }
        }

        return xml;
    }

    private XmlDocument SetReportData(
        XmlDocument xml
        , SolutionInfo reportData
    )
    {
        var elements = xml.GetElementsByTagName(name: Constants.report);
        if (elements.Count > 0)
        {
            var element = elements.Item(index: 0);

            var email = xml.CreateElement(name: Constants.reportItemEmail);
            email.InnerText = reportData.Email;

            var endereco = xml.CreateElement(name: Constants.reportItemEndereco);
            endereco.InnerXml = $"{reportData.Endereco} - {(reportData.UF ?? new UFModel(code: "", name: "")).Code}";

            var nomeEmpresa = xml.CreateElement(name: Constants.reportItemNomeEmpresa);
            nomeEmpresa.InnerText = reportData.NomeEmpresa;

            var responsavel = xml.CreateElement(name: Constants.reportItemResponsavel);
            responsavel.InnerXml = reportData.Responsavel;

            var telefone = xml.CreateElement(name: Constants.reportItemTelefone);
            telefone.InnerXml = reportData.Telefone;

            var data = xml.CreateElement(name: Constants.reportItemData);
            data.InnerXml = reportData.Data.ToString(format: "dd/MM/yyyy");

            var logo = xml.CreateElement(name: Constants.reportItemLogoPath);
            logo.InnerXml = reportData.LogoPath;

            var solutionName = xml.CreateElement(name: Constants.reportItemSolutionName);
            solutionName.InnerXml = reportData.SolutionName;

            element?.AppendChild(newChild: nomeEmpresa);
            element?.AppendChild(newChild: responsavel);
            element?.AppendChild(newChild: email);
            element?.AppendChild(newChild: endereco);
            element?.AppendChild(newChild: telefone);
            element?.AppendChild(newChild: data);
            element?.AppendChild(newChild: logo);
            element?.AppendChild(newChild: solutionName);
        }

        return xml;
    }

    private XmlDocument CreateSolutionStructure(
        XmlDocument document
    )
    {
        var root = document.CreateElement(name: Constants.solutionRoot);
        var report = document.CreateElement(name: Constants.report);
        var projectItems = document.CreateElement(name: Constants.project_items);

        root.AppendChild(newChild: report);
        root.AppendChild(newChild: projectItems);

        document.AppendChild(newChild: root);

        return document;
    }

    private string[] GetDirectoryItems(
        string filePath
    )
    {
        if (Directory.Exists(path: filePath))
        {
            return Directory.GetFiles(path: filePath);
        }

        return new string[] { };
    }

    private void CreateDirectory(
        string filePath
    )
    {
        if (!Directory.Exists(path: filePath))
        {
            Directory.CreateDirectory(path: filePath);
        }
    }
}