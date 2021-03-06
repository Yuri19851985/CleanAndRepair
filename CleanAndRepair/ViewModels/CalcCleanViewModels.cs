using CleanAndRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CleanAndRepair.ViewModels
{
    public class CalcCleanViewModel
    {
        public virtual Service Service { get; set; }
        public CalcCleanParametres Parametres { get; set; }
        public DateTime DateComplete { get; set; }
    }
}