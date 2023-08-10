using AppRepositories.Solution.Dto;

namespace XmlDatasource.Solution.Mappers;

public static class EnderecoItemExtensions
{
    public static EnderecoItem ToEnderecoItem(
        this DTO.EnderecoItem enderecoItem
    ) =>
        new()
        {
            Bairro = enderecoItem.Bairro, Cidade = enderecoItem.Cidade, UF = enderecoItem.UF
            , Logradouro = enderecoItem.Logradouro, Numero = enderecoItem.Numero
        };

    public static DTO.EnderecoItem ToEnderecoItemDto(
        this EnderecoItem enderecoItem
    ) => new()
    {
        Bairro = enderecoItem.Bairro, Cidade = enderecoItem.Cidade, UF = enderecoItem.UF
        , Logradouro = enderecoItem.Logradouro, Numero = enderecoItem.Numero
    };
}