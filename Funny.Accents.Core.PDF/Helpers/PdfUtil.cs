using System.IO;
using Funny.Accents.Core.Types.Position;
using Funny.Accents.Core.Core.PDF.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Funny.Accents.Core.Core.PDF.Helpers
{
    public class PdfUtil
    {
        public static void AddTextToExistingPage(string sourcePdfFile, string destPdfFile
            , int pageToAlter, string[] textValue, int textValueFont
            , IPositionAlignment textValuePosition = default(PositionAlignment)
            , PositionAlignment newPageSize = default)
        {
            var reader = new PdfReader(sourcePdfFile);
            using (var fileStream = new FileStream(destPdfFile, FileMode.Create, FileAccess.Write))
            {
                if (newPageSize == null) { return; }

                var originalPageSize = reader.GetPageSizeWithRotation(1);
                var size = new Rectangle(originalPageSize.Left + newPageSize.LowerX
                    , originalPageSize.Bottom + newPageSize.LowerY
                    , originalPageSize.Right + newPageSize.UpperX
                    , originalPageSize.Top + newPageSize.UpperY);

                var document = new Document(size);
                var writer = PdfWriter.GetInstance(document, fileStream);

                document.Open();

                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    var contentByte = writer.DirectContent;

                    if (i != pageToAlter)
                    {
                        document.NewPage();
                        var originalPage = writer.GetImportedPage(reader, i);
                        contentByte.AddTemplate(originalPage, 0, 0);

                        continue;
                    }

                    document.NewPage();

                    var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    var importedPage = writer.GetImportedPage(reader, i);

                    contentByte.BeginText();
                    contentByte.SetFontAndSize(baseFont, textValueFont);

                    var verticalOffset = 0;
                    foreach (var line in textValue)
                    {
                        contentByte.ShowTextAligned((int)textValuePosition.Alignment, line
                            , textValuePosition.UpperX, textValuePosition.UpperY + verticalOffset, textValuePosition.Rotation);
                        verticalOffset -= 20;
                    }


                    contentByte.EndText();
                    contentByte.AddTemplate(importedPage, newPageSize.UpperX, newPageSize.UpperY);
                }

                document.Close();
                writer.Close();
            }
        }/*End of AddText method*/

        public PositionAlignment GetPdfDimensions(string sourcePdfFile)
        {
            var reader = new PdfReader(sourcePdfFile);
            var originalPageSize = reader.GetPageSizeWithRotation(1);
            return new PositionAlignment(lowerX: originalPageSize.Left, lowerY: originalPageSize.Bottom
                , upperX: originalPageSize.Right, upperY: originalPageSize.Top, rotation: 1);
        }

        //public static void PdfToImage(string targetFilePath, string destinationFilePath,
        //    ImageFormat imageFormat, int xDpi = 300, int yDpi = 300,
        //    Func<string, string> customFileName = null)
        //{
        //    var fileInfo = new FileInfo(targetFilePath);
        //    /*What if the targetFilePath does not exist?*/

        //    var lastInstalledVersion =
        //        GhostscriptVersionInfo.GetLastInstalledVersion(
        //            GhostscriptLicense.GPL | GhostscriptLicense.AFPL,
        //            GhostscriptLicense.GPL);

        //    var rasterizer = new GhostscriptRasterizer();
        //    rasterizer.Open(targetFilePath, lastInstalledVersion, false);

        //    for (var pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
        //    {
        //        var customName = customFileName == null
        //            ? $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}-pageNumber"
        //            : customFileName.Invoke(Path.GetFileNameWithoutExtension(fileInfo.Name));

        //        var pageFilePath = Path.Combine(destinationFilePath, $"{customName}.{imageFormat}");

        //        var img = rasterizer.GetPage(xDpi, yDpi, pageNumber);
        //        img.Save(pageFilePath, imageFormat);
        //    }
        //}/*End of PdfToImage method*/
    }/*End of PdfUtil class*/
}/*End of Funny.Accents.Core.Core.PDF.Helpers namespace*/
