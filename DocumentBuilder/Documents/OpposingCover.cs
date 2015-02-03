using System;
using DocumentBuilder.Models;

namespace DocumentBuilder.Documents
{
    public class OpposingDocumentCover : OpposingDocument
    {
        public OpposingDocumentCover(RouteContext routeContext, OpposingCounsel opposingContext) 
            : base("", routeContext, opposingContext)
        {
        }

        internal override IDocumentFieldsModel Prepare()
        {
            throw new NotImplementedException();
        }
    }
}
