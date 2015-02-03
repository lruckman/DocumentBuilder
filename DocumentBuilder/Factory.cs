using System;
using DocumentBuilder.Models;

namespace DocumentBuilder
{
    public class Factory
    {
        public static DocumentResult Generate<T>(RouteContext routeContext) where T : IDossier
        {
            return ((IDossier)Activator.CreateInstance(typeof(T), routeContext)).Compose();
        }
    }
}