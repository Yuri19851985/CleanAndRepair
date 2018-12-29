using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DAL.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public List<Service> Services { get; set; }
        public User User { get; set; }
        public DateTime DateOrderCheck { get; set; }
        public DateTime DateOrderComplete { get; set; }
        public bool Complete { get; set; }
    }
}