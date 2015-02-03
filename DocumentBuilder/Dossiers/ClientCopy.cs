using DocumentBuilder.Documents;
using DocumentBuilder.Models;

namespace DocumentBuilder.Dossiers
{
    public class ClientCopy : Dossier
    {
        public ClientCopy(RouteContext routeContext) : base(routeContext)
        {
            foreach (var orderLocation in RouteContext.Order.OrderLocations)
            {
                Documents.Add(new CoverSheet(RouteContext));
                //Documents.Add(new Page1(RouteContext, orderLocation));
                //Documents.Add(new Page2(RouteContext, orderLocation));
            }

            var foo = Documents;
        }
    }
}