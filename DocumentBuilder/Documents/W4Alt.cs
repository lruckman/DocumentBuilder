using System;
using DocumentBuilder.Models;

namespace DocumentBuilder.Documents
{
    public class W4Alt : BaseDocument<Request, Location>
    {
        public W4Alt(Request routeContext, Location locationContext)
            : base("DocumentBuilder.Documents.Source.fw4.pdf", routeContext, locationContext)
        {
        }

        internal override IDocumentFillModel Prepare()
        {
            throw new NotImplementedException();
        }
    }
}
