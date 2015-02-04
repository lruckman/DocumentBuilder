using System.Collections.Generic;

namespace DocumentBuilder.Models
{
    public class Request
    {
        public IEnumerable<Location> Locations { get; set; }
    }
}