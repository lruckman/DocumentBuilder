using System.Collections.Generic;

namespace DocumentBuilder.Models
{
    public class RequestContext
    {
        public IEnumerable<LocationContext> Locations { get; set; }
    }
}