using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CleanAndRepair.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Raiting { get; set; }
        public string Phone { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}