using DocumentBuilder.Models;

namespace DocumentBuilder.Documents
{
    public class W4 : BaseDocument<Request>
    {
        public W4(Request routeContext)
            : base("DocumentBuilder.Documents.Source.fw4.pdf", routeContext)
        {
        }

        internal override IDocumentFillModel Prepare()
        {
            return new CoverSheetModel();
        }

        public class CoverSheetModel : IDocumentFillModel
        {

        }
    }
}