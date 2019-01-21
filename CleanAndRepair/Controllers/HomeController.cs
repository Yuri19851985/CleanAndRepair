using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CleanAndRepair.Context;
using CleanAndRepair.Models;
using CleanAndRepair.ViewModels;

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

        public ActionResult BookService(int id)
        {
            var service = db.Services.FirstOrDefault(Service => Service.Id.Equals(id));
            CalcCleanViewModel model = new CalcCleanViewModel();
            if (service != null)
                model.Service = service;
            else return HttpNotFound();
            if (model != null)
                return View(model);
            return HttpNotFound();
        }

       

        [HttpPost]
        public ActionResult BookService(CalcCleanViewModel model)
        {
            if (model != null)
            {
                return View("Zaebis");
            }
            return View("Error");
        }

           
    }
}