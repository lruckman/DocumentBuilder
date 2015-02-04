using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentBuilder.Documents;
using DocumentBuilder.Models;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;

namespace DocumentBuilder.Dossiers
{
    public interface IBaseDossier
    {
        IDocumentResult Compose();
    }

    public abstract class BaseDossier<TRequestContext> : IBaseDossier
    {
        internal readonly TRequestContext RequestContext;

        protected BaseDossier(TRequestContext requestContext)
        {
            RequestContext = requestContext;
            Documents = new List<IDocument>();
        }

        /// <summary>
        ///     Composes all documents into a single PDF file.
        /// </summary>
        /// <returns>A IDocumentResult implementation</returns>
        public IDocumentResult Compose()
        {
            IDocumentResult[] documents = {};

            try
            {
                documents = Documents
                    .Select(x => x.Compose())
                    .ToArray();

                if (!documents.Any())
                {
                    return null;
                }

                var mergedDocument = MergePdfFiles(documents.Select(x => x.Path));

                return new DocumentResult
                {
                    PageCount = documents.Sum(x => x.PageCount),
                    Path = mergedDocument
                };
            }
            finally
            {
                if (documents.Any())
                {
                    TryCleanup(documents
                        .Select(x => x.Path)
                        .ToArray());
                }
            }
        }

        /// <summary>
        ///     A collection of documents we wish to generate
        /// </summary>
        internal IList<IDocument> Documents { get; set; }

        /// <summary>
        ///     Combines a collection of PDF's into a single file
        /// </summary>
        /// <param name="paths">The string collection of PDF file paths to combine.</param>
        /// <returns>A string file path to the new PDF file</returns>
        private static string MergePdfFiles(IEnumerable<string> paths)
        {
            var tempFile = Path.GetTempFileName();

            using (var fileStream = new FileStream(tempFile, FileMode.Append))
            {
                using (var newDocument = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(newDocument, fileStream))
                    {
                        newDocument.Open();

                        var contentByte = writer.DirectContent;

                        PdfImportedPage newPage = null;

                        foreach (var sourceFile in paths)
                        {
                            if (string.IsNullOrWhiteSpace(sourceFile))
                            {
                                continue;
                            }

                            var sourcePdfReader = new PdfReader(sourceFile);
                            var numberOfSourcePages = sourcePdfReader.NumberOfPages;

                            for (var i = 1; i <= numberOfSourcePages; i++)
                            {
                                newPage = writer.GetImportedPage(sourcePdfReader, i);

                                newDocument.SetPageSize(sourcePdfReader.GetPageSize(i));
                                newDocument.NewPage();

                                contentByte.AddTemplate(newPage, 0, 0);
                            }
                        }

                        if (newPage == null)
                        {
                            return null;
                        }

                        newDocument.Close();
                    }
                }
            }

            return tempFile;
        }

        /// <summary>
        ///     File cleanup, will swallow exceptions
        /// </summary>
        /// <param name="paths">The string collection of file paths to delete</param>
        private static void TryCleanup(IEnumerable<string> paths)
        {
            foreach (var path in paths
                .Where(path => !string.IsNullOrWhiteSpace(path)))
            {
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch
                {
                }
            }
        }
    }
}