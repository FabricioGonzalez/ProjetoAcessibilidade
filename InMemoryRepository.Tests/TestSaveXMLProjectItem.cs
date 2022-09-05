using Infrastructure.InMemoryRepository;

using SystemApplication.Services.UIOutputs;

namespace InMemoryRepository.Tests
{
    [TestClass]
    public class TestSaveXMLProjectItem
    {
        private IXmlProjectDataRepository repository;
        public TestSaveXMLProjectItem()
        {
            repository = new XmlProjectDataRepository();
        }
        [TestMethod]
        public void TestSaveData()
        {

            repository.SaveModel(new()
            {
                ItemName = "Test",
                FormData = new()
                {
                new FormDataItemCheckboxModel()
                {
                    Topic = "teste 1",
                    Type = Core.Enums.FormDataItemTypeEnum.Checkbox,
                    Children = new()
                    {
                        new FormDataItemCheckboxChildModel(){
                        Topic = "Teste Opcao",
                        Options = new()
                        {
                            new(){Value = "Sim", IsChecked = true},
                            new(){Value = "Não", IsChecked = false}
,                        },
                            TextItems = new()
{
    new() { Topic = "Tem texto?",MeasurementUnit = "m",TextData = "TESTE",Type = Core.Enums.FormDataItemTypeEnum.Text}
}
                        }
                    }
                },
                new FormDataItemTextModel()

                     { Topic = "Tem texto?",MeasurementUnit = "m",TextData = "TESTE",Type = Core.Enums.FormDataItemTypeEnum.Text},

                  new FormDataItemObservationModel()
                        {Observation = "TESTE",
                        Topic = "Observação",
                        Type = Core.Enums.FormDataItemTypeEnum.Observation

                        }
                }
            }, Path.Combine("C:\\Users\\Kurog\\Desktop","test.xml"));
        }
    }
}