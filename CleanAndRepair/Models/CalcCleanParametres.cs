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
        public string Strong { get; set; }
        public string Medium { get; set; }
        public string Easy { get; set; }
    }
}

