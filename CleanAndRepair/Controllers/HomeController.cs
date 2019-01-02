using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Entities;
using DAL.Context;

namespace CleanAndRepair.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        public ActionResult ShowServices(int id)
        {
            var srv = db.Services.Where(i => i.GroupId == id);
            if (srv != null)
                return PartialView(srv);
            return HttpNotFound();
        }


    }
}