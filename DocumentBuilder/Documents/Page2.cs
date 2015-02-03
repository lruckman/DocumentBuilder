using System;
using DocumentBuilder.Models;

namespace DocumentBuilder.Documents
{
    public class Page2 : LocationDocument
    {
        public Page2(RouteContext routeContext, OrderLocation locationContext)
            : base("", routeContext, locationContext)
        {
        }

        internal override IDocumentFieldsModel Prepare()
        {
            throw new NotImplementedException();
        }
    }
}