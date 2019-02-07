using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CleanAndRepair.Models
{
    public class Order
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Service ServiceOrder { get; set; }
        public DateTime DateOrderCheck { get; set; }
        public DateTime DateOrderComplete { get; set; }
        public bool Complete { get; set; }
        public double TotalPrice { get; set; }
    }
}