using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CleanAndRepair.Models
{
    public class GroupService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Service> Services { get; set; }
    }
}