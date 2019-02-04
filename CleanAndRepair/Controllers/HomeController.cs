using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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

       // Рачет стоимости уборки помещения
       public double CalcClean (Service service, CalcCleanParametres parametres)
        {
            //вычисляем коэффициент загрязненности в зависимости от чекбоксов
            double KoeffEasy = parametres.Easy =="Easy" ? 0.9 : 0;
            double KoeffMedium = parametres.Medium == "Medium" ? 1 : 0;
            double KoeffStrong = parametres.Strong == "Strong" ? 1.3 : 0;

            double Norma = 15; // условная норма 15 м.кв 1 нормочас 
            double CountHours;

            if (service != null && parametres != null)
            {
                //расчет количества нормочасов 
                CountHours = parametres.RoomSquare / Norma * (KoeffEasy + KoeffMedium + KoeffStrong);

                return service.Price * CountHours;                 
            }
            return 0;
        }

        [HttpPost]
        public ActionResult BookService(CalcCleanViewModel model)
        {
            Order NewOrder = new Order();
            NewOrder.ServiceOrder = model.Service;

            NewOrder.DateOrderCheck = DateTime.Now;
            NewOrder.DateOrderComplete = new DateTime(2019, 2, 28);
            NewOrder.TotalPrice = CalcClean(model.Service, model.Parametres);
            // получаем текущего юзера
            string NameCurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
            if (NameCurrentUser != null)
            {
                NewOrder.User = new ApplicationUser() { UserName = NameCurrentUser };
            }
            if (NewOrder != null)
            {
                return View("ServiceOrderConfirm", NewOrder);
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult BookServiceComplete (Order NewOrder)
        {

            // добавляем авторизованному юзеру позицию заказа
            string NameCurrentUser;
            if (System.Web.HttpContext.Current != null &&
                System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                NameCurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
                if (NameCurrentUser != null)
                {
                    var CurrentUser = db.Users.FirstOrDefault(User => User.UserName.Equals(NameCurrentUser));
                    if(CurrentUser!=null)
                    {
                        CurrentUser.Orders = new List<Order>();
                        CurrentUser.Orders.Add(NewOrder);
                        db.SaveChanges();
                        ViewBag.UserName = NameCurrentUser;
                        return View("BookServiceComplete");
                    }
                   
                }
            }
            return View("Error");
        }

        public ActionResult ViewOrderList ()
        {
            return View("Zaebis");
        }

    }
}