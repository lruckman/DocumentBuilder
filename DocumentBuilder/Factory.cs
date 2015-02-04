using System;
using DocumentBuilder.Dossiers;
using DocumentBuilder.Models;

namespace DocumentBuilder
{
    public class Factory
    {
        //public static IDocumentResult Generate<TDossier, TRequestContext>(TRequestContext requestContext)
        //    where TDossier : IBaseDossier<TRequestContext>
        //{
        //    return ((IBaseDossier)Activator.CreateInstance(typeof(TDossier), requestContext)).Compose();
        //}

        public static IDocumentResult GenerateClientCopy(Request requestContext)
        {
            return new ClientCopy(requestContext).Compose();
        }
    }
}