using System;
using DocumentBuilder.Models;

namespace DocumentBuilder.Documents
{
    public class W4Alt : BaseDocument<RequestContext, LocationContext>
    {
        public W4Alt(RequestContext routeContext, LocationContext locationContext)
            : base("DocumentBuilder.Documents.Source.fw4.pdf", routeContext, locationContext)
        {
        }

        internal override IDocumentFillModel Prepare()
        {
            throw new NotImplementedException();
        }
    }
}
