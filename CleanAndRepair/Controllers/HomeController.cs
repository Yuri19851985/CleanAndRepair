using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CleanAndRepair.Context;
using CleanAndRepair.Models;

namespace CleanAndRepair.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
            
        }

        public ActionResult ShowServices(int id)
        {
            var gr = db.Groups.FirstOrDefault(Group => Group.Id.Equals(id));     // Where(i => i.GroupId == id);
            if (gr != null)
                return PartialView(gr.Services);
            return HttpNotFound();
        }

        public ActionResult ServiceClean()
        {
            return View(db.Groups.ToList());
        }
    }
}