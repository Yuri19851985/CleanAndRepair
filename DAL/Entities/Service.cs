using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DAL.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; } // цена за один нормочас
        public double Count { get; set; } // количество нормочасов, величина расчетная
        public int GroupId { get; set; }
        virtual public GroupService Group { get; set; }
    }
}