using CleanAndRepair.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CleanAndRepair.ViewModels
{
    public class UserListViewModel
    {
        public virtual ApplicationUser user { get; set; }
        public int CountOrders { get; set; }
    }

    public class UserEditViewModel
    {
        public virtual ApplicationUser user { get; set; }
        public List<IdentityRole> Roles = new List<IdentityRole>();
    }

    public class UserDetailsViewModel
    {
        public virtual ApplicationUser user { get; set; }
        public List<Order> Orders = new List<Order>();
    }

    public class ServiceViewModel
    {
        public virtual Service service { get; set; }
        public List<GroupService> Groups = new List<GroupService>();
    }

    

}