using System;
using System.IO;
using System.Linq;
using DocumentBuilder.Models;
using iTextSharp.text.pdf;

namespace DocumentBuilder
{
    public interface IDocument
    {
        DocumentResult Compose();
    }

    public abstract class Document : IDocument
    {
        internal readonly RouteContext RouteContext;
        private readonly string _sourceFileName;

        protected Document(string sourceFileName, RouteContext routeContext)
        {
            _sourceFileName = sourceFileName;
            RouteContext = routeContext;
        }

        public DocumentResult Compose()
        {
            return SetFields(Prepare());
        }

        private DocumentResult SetFields(IDocumentFieldsModel model)
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
                            var stamp =
                                propertyInfo.GetCustomAttributes(typeof (StampAttribute), false).First() as
                                    StampAttribute;
                            var propertyType = propertyInfo.PropertyType;
                            var value = propertyInfo.GetValue(model);

                            if (propertyType.Name.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                            {
                                var dateValue = DateTime.Parse(value.ToString());
                                if (dateValue != DateTime.MinValue)
                                    pdfStamper.AcroFields.SetField(stamp.FieldName, dateValue.ToString(stamp.Format));
                            }
                            else if (propertyType.Name.Equals("bool", StringComparison.OrdinalIgnoreCase) ||
                                     propertyType.Name.Equals("boolean", StringComparison.OrdinalIgnoreCase))
                            {
                                pdfStamper.AcroFields.SetField(stamp.FieldName,
                                    bool.Parse(value.ToString()) ? "Yes" : "No");
                            }
                            else if (propertyType.Name.Equals("int32", StringComparison.OrdinalIgnoreCase))
                            {
                                pdfStamper.AcroFields.SetField(stamp.FieldName, value.ToString());
                            }
                            else if (propertyType.Name.Equals("decimal", StringComparison.OrdinalIgnoreCase))
                            {
                                var decimalValue = decimal.Parse(value.ToString());
                                pdfStamper.AcroFields.SetField(stamp.FieldName, decimalValue.ToString(stamp.Format));
                            }
                            else
                            {
                                pdfStamper.AcroFields.SetField(stamp.FieldName,
                                    value == null ? string.Empty : value as string);
                            }
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

        internal abstract IDocumentFieldsModel Prepare();
    }
}