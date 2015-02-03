using DocumentBuilder.Models;

namespace DocumentBuilder
{
    public abstract class LocationDocument : Document
    {
        internal readonly OrderLocation LocationContext;

        protected LocationDocument(string sourceFileName, RouteContext routeContext, OrderLocation locationContext)
            : base(sourceFileName, routeContext)
        {
            LocationContext = locationContext;
        }
    }
}