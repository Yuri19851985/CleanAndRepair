using CleanAndRepair.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CleanAndRepair.Models.Entities;


namespace CleanAndRepair.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DbConnectionString", throwIfV1Schema: false)
        {
            Database.SetInitializer(new Initializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<GroupService> Groups { get; set; }
    }
}