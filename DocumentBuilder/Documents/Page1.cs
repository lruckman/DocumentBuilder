using System;
using DocumentBuilder.Models;

namespace DocumentBuilder.Documents
{
    public class Page1 : LocationDocument
    {
        public Page1(RouteContext routeContext, OrderLocation locationContext)
            : base("", routeContext, locationContext)
        {
        }

        internal override IDocumentFieldsModel Prepare()
        {
            throw new NotImplementedException();
        }
    }
}