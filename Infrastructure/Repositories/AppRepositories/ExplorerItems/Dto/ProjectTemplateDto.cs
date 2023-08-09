namespace AppRepositories.ExplorerItems.Dto;

public record ProjectTemplateDto(
    string Id
    , string Name
    , string TemplateName
    , string LocationPath
    , int Version
);