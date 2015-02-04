using System;
using DocumentBuilder.Dossiers;
using DocumentBuilder.Models;

namespace DocumentBuilder
{
    public class Factory
    {
        public static IDocumentResult Generate<T>(RequestContext routeContext) where T : IDossier
        {
            return ((IDossier)Activator.CreateInstance(typeof(T), routeContext)).Compose();
        }
    }
}