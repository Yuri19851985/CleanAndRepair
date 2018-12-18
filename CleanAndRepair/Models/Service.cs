using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CleanAndRepair.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Professiona> Workers { get; set; }
    }
}