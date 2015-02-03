using System;
using DocumentBuilder.Models;

namespace DocumentBuilder.Documents
{
    public class CoverSheet : Document
    {
        public CoverSheet(RouteContext routeContext)
            : base("DocumentBuilder.Documents.Source.CoverSheet.pdf", routeContext)
        {
        }

        internal override IDocumentFieldsModel Prepare()
        {
            return new CoverSheetModel();
        }

        public class CoverSheetModel : IDocumentFieldsModel
        {
            
        }
    }
}
