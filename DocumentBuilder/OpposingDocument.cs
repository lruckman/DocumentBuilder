using DocumentBuilder.Models;

namespace DocumentBuilder
{
    public abstract class OpposingDocument : Document
    {
        internal readonly OpposingCounsel OpposingContext;

        protected OpposingDocument(string sourceFileName, RouteContext routeContext, OpposingCounsel opposingContext)
            : base(sourceFileName, routeContext)
        {
            OpposingContext = opposingContext;
        }
    }
}