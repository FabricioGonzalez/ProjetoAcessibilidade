using ITextSharpReport;

namespace ITextSharpTest;

[TestClass]
public class PDFCreation
{
    //TODO 
    /*
     Test case to create PDF
     */


    private GerarPDF pdfGenerator;

    [TestMethod]
    public void CreatePDF()
    {
        pdfGenerator = new();
    }
}