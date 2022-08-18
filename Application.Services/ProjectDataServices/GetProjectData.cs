using System.Collections.ObjectModel;

using SystemApplication.Services.Contracts;
using SystemApplication.Services.UIOutputs;

namespace SystemApplication.Services.ProjectDataServices;
public class GetProjectData
{
    readonly IXmlProjectDataRepository xmlProjectrepository;
    readonly IProjectSolutionRepository projectSolutionRepository;
    public GetProjectData(IXmlProjectDataRepository xmlProjectrepository, IProjectSolutionRepository projectSolutionRepository)
    {
        this.xmlProjectrepository = xmlProjectrepository;
        this.projectSolutionRepository = projectSolutionRepository;
    }
    public ItemModel GetItemProject(string path)
    {
        var result = xmlProjectrepository.GetModel(path);

        var item = new ItemModel();

        item.ItemName = result.ItemName;
        item.FormData = new();
        item.LawList = new();

        foreach (var itemTable in result.FormData)
        {
            if (itemTable.Type == Core.Enums.FormDataItemTypeEnum.Checkbox)
            {
                var newitem = new UIOutputs.FormDataItemCheckboxModel()
                {
                    Topic = itemTable.Topic,
                    Children = new(),
                    Type = itemTable.Type
                };
                var itens = (itemTable as Core.Models.FormDataItemCheckboxModel).Children;

                itens.ForEach((e) =>
                {
                    var child = new UIOutputs.FormDataItemCheckboxChildModel()
                    {
                        Options = new(),
                        TextItems = new()
                    };

                    child.Topic = e.Topic;

                    e.Options.ForEach(i =>
                    {
                        var itemOption = new UIOutputs.OptionModel();
                        itemOption.Value = i.Value;
                        itemOption.IsChecked = i.IsChecked;

                        child.Options.Add(itemOption);
                    });

                    e.TextItems.ForEach(i =>
                    {
                        var textItem = new UIOutputs.FormDataItemTextModel();

                        textItem.TextData = i.TextData;
                        textItem.MeasurementUnit = i.MeasurementUnit;
                        textItem.Topic = i.Topic;
                        textItem.Type = i.Type;

                        child.TextItems.Add(textItem);

                    });

                    newitem.Children.Add(child);
                });

                item.FormData.Add(newitem);
            }

            if (itemTable.Type == Core.Enums.FormDataItemTypeEnum.Text)
            {
                var newTextItem = new UIOutputs.FormDataItemTextModel();

                newTextItem.Topic = itemTable.Topic;
                newTextItem.TextData = (itemTable as Core.Models.FormDataItemTextModel).TextData;
                newTextItem.MeasurementUnit = (itemTable as Core.Models.FormDataItemTextModel).MeasurementUnit;
                newTextItem.Type = itemTable.Type;

                item.FormData.Add(newTextItem);
            }

            if (itemTable.Type == Core.Enums.FormDataItemTypeEnum.Observation)
            {
                var newObservationItem = new UIOutputs.FormDataItemObservationModel();

                newObservationItem.Topic = itemTable.Topic;
                newObservationItem.Observation = (itemTable as Core.Models.FormDataItemObservationModel).Observation;
                newObservationItem.Type = itemTable.Type;

                item.FormData.Add(newObservationItem);
            }
        }

        foreach (var law in result.LawList)
        {
            item.LawList.Add(new()
            {
                LawId = law.LawId,
                LawTextContent = law.LawTextContent,
            });
        }

        return item;
    }

    public async Task<List<FileTemplates>> GetProjectItens()
    {
        return await projectSolutionRepository.getProjectLocalPath();
    } 

    public async Task<ObservableCollection<ExplorerItem>> GetProjectSolutionItens(string solutionPath)
    {
        return await projectSolutionRepository.GetData(solutionPath);
    }
}
