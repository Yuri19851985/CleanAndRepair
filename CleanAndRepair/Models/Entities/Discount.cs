using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CleanAndRepair.Models.Entities
{
    public class Discount
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
        public double KoeffPrice { get; set; }
        public Service Service { get; set; }
    }
}