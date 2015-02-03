using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentBuilder.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DocumentBuilder
{
    public interface IDossier
    {
        DocumentResult Compose();
    }

    public abstract class Dossier : IDossier
    {
        internal readonly RouteContext RouteContext;

        protected Dossier(RouteContext routeContext)
        {
            RouteContext = routeContext;
            Documents = new List<IDocument>();
        }

        public DocumentResult Compose()
        {
            var documents = Documents.Select(x => x.Compose());
            var paths = documents.Select(x => x.Path);

            if (!paths.Any())
            {
                return null;
            }

            var mergedDocument = MergePdfBytes(paths);

            return new DocumentResult
            {
                PageCount = documents.Sum(x => x.PageCount),
                Path = mergedDocument
            };
        }

        internal IList<IDocument> Documents { get; set; }

        public static string MergePdfBytes(IEnumerable<string> sourceFiles)
        {
            var tempFile = Path.GetTempFileName();

            using (var fileStream = new FileStream(tempFile, FileMode.Append))
            {
                using (var document = new iTextSharp.text.Document())
                {
                    using (var writer = PdfWriter.GetInstance(document, fileStream))
                    {
                        document.SetPageSize(PageSize.LETTER);
                        document.Open();

                        var contentByte = writer.DirectContent;

                        PdfImportedPage page = null;

                        foreach (var sourceFile in sourceFiles)
                        {
                            if (string.IsNullOrWhiteSpace(sourceFile))
                            {
                                continue;
                            }

                            var reader = new PdfReader(sourceFile);
                            var pages = reader.NumberOfPages;

                            for (var j = 1; j <= pages; j++)
                            {
                                document.SetPageSize(PageSize.LETTER);
                                document.NewPage();
                                page = writer.GetImportedPage(reader, j);
                                contentByte.AddTemplate(page, 0, 0);
                            }
                        }
                        if (page == null)
                        {
                            return null;
                        }

                        document.Close();
                    }
                }
            }

            return tempFile;
        }
    }
}