using System.Collections.Generic;

namespace DocumentBuilder.Models
{
    public class Order
    {
        public Order()
        {
            OpposingCounsels = new OpposingCounsel[] {};
            OrderLocations = new OrderLocation[] {};
        }

        public IEnumerable<OpposingCounsel> OpposingCounsels { get; set; }

        public IEnumerable<OrderLocation> OrderLocations { get; set; } 
    }
}