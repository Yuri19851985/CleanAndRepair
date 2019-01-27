using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CleanAndRepair.Models
{
    public class CalcCleanParametres
    {
        public int Id { get; set; }
        public double RoomSquare { get; set; }
        public int CountRooms { get; set; }
        public int CleanLevel { get; set; }
        public bool Strong { get; set; }
        public bool Medium { get; set; }
        public bool Easy { get; set; }
    }
}

