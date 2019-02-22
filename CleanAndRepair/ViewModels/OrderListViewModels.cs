using CleanAndRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CleanAndRepair.ViewModels
{
    public class OrderListViewModels
    {
        public virtual Order Order { get; set; }
        public virtual Service Service { get; set; }
    }
}