
using AppUsecases.App.Models;

namespace AppUsecases.App.Usecases;
public class GetUFList
{
    public List<UF> GetAllUF()
    {
        return new List<UF>()
        {
new UF("Rondônia","RO"),
new UF("Acre","AC"),
new UF("Amazonas","AM"),
new UF("Roraima","RR"),
new UF("Pará","PA"),
new UF("Amapá","AP"),
new UF("Tocantins","TO"),
new UF("Maranhão","MA"),
new UF("Piauí","PI"),
new UF("Ceará","CE"),
new UF("Rio Grande do Norte","RN"),
new UF("Paraíba","PB"),
new UF("Pernambuco","PE"),
new UF("Alagoas","AL"),
new UF("Sergipe","SE"),
new UF("Bahia","BA"),
new UF("Minas Gerais","MG"),
new UF("Espírito Santo","ES"),
new UF("Rio de Janeiro","RJ"),
new UF("São Paulo","SP"),
new UF("Paraná","PR"),
new UF("Santa Catarina","SC"),
new UF("Rio Grande do Sul","RS"),
new UF("Mato Grosso do Sul","MS"),
new UF("Mato Grosso","MT"),
new UF("Goiás","GO"),
new UF("Distrito Federal","DF")
        };
    }
}
