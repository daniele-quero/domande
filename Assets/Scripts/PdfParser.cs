
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text;

public class PdfParser
{
    public static string ReadPdfFile(string filePath)
    {
        using (PdfReader pdfReader = new PdfReader(filePath))
        using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
        {
            StringBuilder text = new StringBuilder();

            for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
            {
                var page = pdfDocument.GetPage(i);
                var strategy = new SimpleTextExtractionStrategy();
                var currentText = PdfTextExtractor.GetTextFromPage(page, strategy);
                text.Append(currentText);
            }

            return text.ToString();
        }
    }
}
