using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DAL.Context
{
    public class Context:DbContext
    {
        public Context() : base("DbConnectionString")
        {
        }
        static Context()
        {
            Database.SetInitializer(new Initializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<GroupService> Groups { get; set; }
    }
}