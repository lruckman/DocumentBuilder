using DocumentBuilder.Documents;
using DocumentBuilder.Models;

namespace DocumentBuilder.Dossiers
{
    public class ClientCopy : Dossier
    {
        public ClientCopy(RequestContext requestContext) : base(requestContext)
        {
            foreach (var location in RequestContext.Locations)
            {
                Documents.Add(new W4(RequestContext));
            }
        }
    }
}