using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace ITextSharpReport;
public class GerarPDF
{
    public const String DEST = "results/example.pdf";

    BaseFont fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

    readonly float pxPorMm = 72 / 25.2f;

    public GerarPDF()
    {
        FileInfo file = new FileInfo(string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"/", DEST));
        file.Directory.Create();
        createPdf(file.FullName);
    }

    public PdfPTable CreateHeader()
    {
        var headerTable = new PdfPTable(4);
        headerTable.HeaderRows = 0;
        GenerateCell(" ", headerTable);
        GenerateCell("Local", headerTable);
        GenerateCell("UF", headerTable);
        GenerateCell("Data", headerTable);

        GenerateCell(" ", headerTable);
        GenerateCell("Local", headerTable);
        GenerateCell("UF", headerTable);
        GenerateCell("Data", headerTable);

        GenerateCell("REVISÃO", headerTable);
        GenerateCell("Empresa", headerTable, colSpan: 2);
        GenerateCell("PAVIMENTO", headerTable);

        return headerTable;
    }

    private void GenerateCell(string Texto, PdfPTable headerTable, int colSpan = 1, int rowSpan = 1)
    {
        Font fontCelula = new Font(fonteBase, 8, Font.NORMAL, BaseColor.Black);

        PdfPCell cell = new PdfPCell(new Phrase(Texto, fontCelula));

        cell.FixedHeight = 25;
        cell.BorderWidth = 0;
        cell.BorderWidthBottom = 1;
        cell.HorizontalAlignment = Element.ALIGN_CENTER;

        cell.Colspan = colSpan;
        cell.Rowspan = rowSpan;

        headerTable.AddCell(cell);
    }

    public void createPdf(String dest)
    {
        FileStream fs = new FileStream(dest, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

        Document document = new Document(PageSize.Letter);
        PdfWriter writer = PdfWriter.GetInstance(document, fs);
        document.Open();

        TOCEvent evento = new TOCEvent();
        writer.PageEvent = evento;

        document.Add(CreateHeader());

        for (int i = 0; i < 10; i++)
        {
            var title = "This is title " + i;
            Chunk c = new Chunk(title, new Font());
            c.SetGenericTag(title);
            document.Add(new Paragraph(c));
            for (int j = 0; j < 50; j++)
            {
                document.Add(new Paragraph("Line " + j + " of title " + i + " page: " + writer.PageNumber));
            }
        }
        document.NewPage();
        document.Add(new Paragraph("Table of Contents", new Font()));
        Chunk dottedLine = new Chunk(new DottedLineSeparator());
        List<PageIndex> entries = evento.getTOC();

        MultiColumnText columns = new MultiColumnText();
        columns.AddRegularColumns(72, 72 * 7.5f, 24, 2);

        Paragraph p;
        for (int i = 0; i < 10; i++)
        {
            foreach (PageIndex pageIndex in entries)
            {
                Chunk chunk = new Chunk(pageIndex.Text);
                chunk.SetAction(PdfAction.GotoLocalPage(pageIndex.Name, false));
                p = new Paragraph(chunk);
                p.Add(dottedLine);

                chunk = new Chunk(pageIndex.Page.ToString());
                chunk.SetAction(PdfAction.GotoLocalPage(pageIndex.Name, false));
                p.Add(chunk);

                columns.AddElement(p);
            }
        }
        document.Add(columns);

        document.Close();
    }

    public class TOCEvent : PdfPageEventHelper
    {
        protected int counter = 0;
        protected List<PageIndex> toc = new List<PageIndex>();

        public override void OnGenericTag(PdfWriter writer, Document document, Rectangle rect, string text)
        {
            String name = "dest" + (counter++);
            int page = writer.PageNumber;
            toc.Add(new PageIndex() { Text = text, Name = name, Page = page });
            writer.DirectContent.LocalDestination(name, new PdfDestination(PdfDestination.FITH, rect.GetTop(0)));
        }

        public List<PageIndex> getTOC()
        {
            return toc;
        }
    }
}

public class PageIndex
{
    public string Text
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public int Page
    {
        get; set;
    }
}

class PDFBackgroundHelper : PdfPageEventHelper
{
    private PdfContentByte cb;
    private List<PdfTemplate> templates;
    //constructor
    public PDFBackgroundHelper()
    {
        templates = new List<PdfTemplate>();
    }

    public override void OnEndPage(PdfWriter writer, Document document)
    {
        base.OnEndPage(writer, document);

        cb = writer.DirectContentUnder;
        PdfTemplate templateM = cb.CreateTemplate(50, 50);
        templates.Add(templateM);

        int pageN = writer.CurrentPageNumber;
        String pageText = $"Página {pageN}";
        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        float len = bf.GetWidthPoint(pageText, 5);
        cb.BeginText();
        cb.SetFontAndSize(bf, 5);
        cb.SetTextMatrix(document.LeftMargin, document.PageSize.GetBottom(document.BottomMargin));
        cb.ShowText(pageText);
        cb.EndText();
        cb.AddTemplate(templateM, document.LeftMargin + len, document.PageSize.GetBottom(document.BottomMargin));
    }

    public override void OnCloseDocument(PdfWriter writer, Document document)
    {
        base.OnCloseDocument(writer, document);
        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        foreach (PdfTemplate item in templates)
        {
            item.BeginText();
            item.SetFontAndSize(bf, 5);
            item.SetTextMatrix(0, 0);
            item.ShowText("" + (writer.PageNumber));
            item.EndText();
        }

    }
}