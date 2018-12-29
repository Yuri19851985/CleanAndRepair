using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Entities
{
    public class Professional
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Raiting { get; set; }
        public List<Service> Services { get; set; }
    }
}