using System.Collections.Generic;

namespace DocumentBuilder.Models
{
    public class RouteContext
    {
        public RouteContext()
        {
            Order = new Order();
            OrderLocation = new OrderLocation();
        }

        public Order Order { get; set; }
        public OrderLocation OrderLocation { get; set; }
        public OpposingCounsel OpposingCounsel { get; set; } 
    }
}