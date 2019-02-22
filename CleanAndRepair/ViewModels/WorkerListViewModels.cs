using CleanAndRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CleanAndRepair.ViewModels
{
    public class WorkerListViewModel
    {
        public virtual Worker worker { get; set; }
        public int CountOrders { get; set; }
    }

    public class WorkerDetailsViewModel
    {
        public virtual Worker worker { get; set; }
        public List<Order> Orders = new List<Order>();
    }

    public class ServiceViewModel
    {
        public virtual Service service { get; set; }
        public List<GroupService> Groups = new List<GroupService>();
    }
}