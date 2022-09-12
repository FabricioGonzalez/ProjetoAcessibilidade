using System.Collections.ObjectModel;

using Projeto.Core.Models;

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
    public async Task<ItemModel> GetItemProject(string path)
    {
        var result = await xmlProjectrepository.GetModel(path);

        var item = new ItemModel();

        item.ItemName = result.ItemName;
        item.FormData = new();
        item.LawList = new();

        foreach (var itemTable in result.FormData)
        {
            if (itemTable.Type == Core.Enums.FormDataItemTypeEnum.Checkbox)
            {
                var newitem = new FormDataItemCheckboxModel()
                {
                    Topic = itemTable.Topic,
                    Children = new(),
                    Type = itemTable.Type
                };
                var itens = (itemTable as FormDataItemCheckboxModel).Children;

                itens.ForEach((e) =>
                {
                    var child = new FormDataItemCheckboxChildModel()
                    {
                        Options = new(),
                        TextItems = new()
                    };

                    child.Topic = e.Topic;

                    e.Options.ForEach(i =>
                    {
                        var itemOption = new OptionModel();
                        itemOption.Value = i.Value;
                        itemOption.IsChecked = i.IsChecked;

                        child.Options.Add(itemOption);
                    });

                    e.TextItems.ForEach(i =>
                    {
                        var textItem = new FormDataItemTextModel();

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
                var newTextItem = new FormDataItemTextModel();

                newTextItem.Topic = itemTable.Topic;
                newTextItem.TextData = (itemTable as FormDataItemTextModel).TextData;
                newTextItem.MeasurementUnit = (itemTable as FormDataItemTextModel).MeasurementUnit;
                newTextItem.Type = itemTable.Type;

                item.FormData.Add(newTextItem);
            }

            if (itemTable.Type == Core.Enums.FormDataItemTypeEnum.Observation)
            {
                var newObservationItem = new FormDataItemObservationModel();

                newObservationItem.Topic = itemTable.Topic;
                newObservationItem.Observation = (itemTable as FormDataItemObservationModel).Observation;
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

    public async Task<ProjectSolutionModel>? GetProjectSolution(string solutionPath)
    {
     return await projectSolutionRepository.GetProjectSolutionData(solutionPath);
    }
    public async Task<List<FileTemplates>> GetProjectItens()
    {
        try
        {
            return await projectSolutionRepository.getProjectLocalPath();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ObservableCollection<ExplorerItem>> GetProjectSolutionItens(string solutionPath)
    {
        return await projectSolutionRepository.GetData(solutionPath);
    }
}
