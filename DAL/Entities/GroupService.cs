using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Entities
{
    public class GroupService
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public List<Service> Services { get; set; }
    }
}