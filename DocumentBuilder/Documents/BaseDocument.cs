using System;
using System.IO;
using System.Linq;
using DocumentBuilder.Models;
using iTextSharp.text.pdf;

namespace DocumentBuilder.Documents
{
    public interface IDocument
    {
        IDocumentResult Compose();
    }

    public abstract class BaseDocument<TRequestContext> : IDocument
    {
        internal readonly TRequestContext RequestContext;
        private readonly string _sourceFileName;

        protected BaseDocument(string sourceFileName, TRequestContext requestContext)
        {
            _sourceFileName = sourceFileName;
            RequestContext = requestContext;
        }

        public IDocumentResult Compose()
        {
            return SetFields(Prepare());
        }

        private IDocumentResult SetFields(IDocumentFillModel model)
        {
            using (var reader = new PdfReader(ExtractResource(_sourceFileName)))
            {
                var tempFile = Path.GetTempFileName();

                using (var fileStream = new FileStream(tempFile, FileMode.Create))
                {
                    using (var pdfStamper = new PdfStamper(reader, fileStream))
                    {
                        pdfStamper.FormFlattening = true;

                        foreach (var propertyInfo in model.GetType()
                            .GetProperties()
                            .Where(x => Attribute.IsDefined(x, typeof (StampAttribute), false))
                            .ToArray())
                        {
                            var stamp = (StampAttribute) propertyInfo
                                .GetCustomAttributes(typeof (StampAttribute), false)
                                .First();

                            var propertyType = propertyInfo.PropertyType;
                            var propertyName = propertyType.Name.ToUpperInvariant();
                            var value = propertyInfo.GetValue(model);

                            if (String.IsNullOrWhiteSpace(Convert.ToString(value)))
                            {
                                continue;
                            }

                            if (propertyName == "DATETIME")
                            {
                                var dateValue = DateTime.Parse(value.ToString());
                                if (dateValue != DateTime.MinValue)
                                {
                                    pdfStamper.AcroFields.SetField(stamp.FieldName, dateValue.ToString(stamp.Format));
                                }
                                continue;
                            }

                            if (propertyName == "BOOL" || propertyName == "BOOLEAN")
                            {
                                pdfStamper.AcroFields.SetField(stamp.FieldName,
                                    bool.Parse(value.ToString()) ? "Yes" : "No");

                                continue;
                            }

                            if (propertyName == "INT32")
                            {
                                pdfStamper.AcroFields.SetField(stamp.FieldName,
                                    value.ToString());

                                continue;
                            }

                            if (propertyName == "DECIMAL")
                            {
                                var decimalValue = decimal.Parse(value.ToString());

                                pdfStamper.AcroFields.SetField(stamp.FieldName,
                                    decimalValue.ToString(stamp.Format));

                                continue;
                            }

                            pdfStamper.AcroFields.SetField(stamp.FieldName,
                                value == null ? string.Empty : value as string);
                        }

                        pdfStamper.Close();

                        return new DocumentResult
                        {
                            PageCount = reader.NumberOfPages,
                            Path = tempFile
                        };
                    }
                }
            }
        }

        private byte[] ExtractResource(string resource)
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream(resource))
            {
                var bytes = new byte[(int) stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                return bytes;
            }
        }

        internal abstract IDocumentFillModel Prepare();
    }

    public abstract class BaseDocument<TRequestContext, TDataContext> : BaseDocument<TRequestContext>
    {
        internal readonly TDataContext DataContext;

        protected BaseDocument(string sourceFileName, TRequestContext requestContext, TDataContext dataContext)
            : base(sourceFileName, requestContext)
        {
            DataContext = dataContext;
        }
    }
}