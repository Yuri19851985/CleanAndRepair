using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CleanAndRepair.Context;
using CleanAndRepair.Models;
using CleanAndRepair.ViewModels;
using Microsoft.AspNet.Identity;

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

        // выполняется если пользователь выбрал услугу из группы "Уборка"
        [Authorize(Roles = "user")]
        public ActionResult BookServiceClean(int id)
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
            double KoeffEasy = parametres.CleanLevel =="Easy" ? 0.9 : 0;
            double KoeffMedium = parametres.CleanLevel == "Medium" ? 1 : 0;
            double KoeffStrong = parametres.CleanLevel == "Strong" ? 1.3 : 0;

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

        [Authorize(Roles = "user")]
        public ActionResult BookService(int id)
        {
            var service = db.Services.FirstOrDefault(Service => Service.Id.Equals(id));
            if (service != null)
            {
                var GroupId = service.Group.Id;
                if (GroupId == 1)
                {
                    return RedirectToAction("BookServiceClean", new { id });
                }
            }
            CalcCleanViewModel model = new CalcCleanViewModel();
            if (service != null)
                model.Service = service;
            else return HttpNotFound();
            if (model != null)
                return View(model);
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult BookService(CalcCleanViewModel model)
        {
            //если дата введена неверно (по-хорошему надо сделать с валидаторами прямо на странице
            if(model.DateComplete <= DateTime.Now)
            {
                return RedirectToAction("BookService", new { model.Service.Id});
            }
            // если дата введена верно
            Order NewOrder = new Order();
            NewOrder.ServiceOrder = model.Service;

            NewOrder.DateOrderCheck = DateTime.Now;
            NewOrder.DateOrderComplete = model.DateComplete;
            //проверяю из группы "уборка" или нет
            var service = db.Services.FirstOrDefault(s => s.Id == model.Service.Id);
            if (service != null)
            {
                var GroupId = service.Group.Id;
                if (GroupId == 1)
                {
                    NewOrder.TotalPrice = CalcClean(model.Service, model.Parametres);
                }
                else NewOrder.TotalPrice = model.Service.Price;
            }
            else return View("Error. Service not found!");
            
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
        [Authorize(Roles = "user")]
        public ActionResult BookServiceComplete (Order NewOrder)
        {
            var CurrentOrderService = db.Services.FirstOrDefault(service => service.Name == NewOrder.ServiceOrder.Name);
            if(CurrentOrderService != null)
            {
                NewOrder.ServiceOrder = CurrentOrderService;
                NewOrder.Complete = false;
            }
            // добавляем авторизованному юзеру позицию заказа
            string currentUserId = User.Identity.GetUserId();

            if (currentUserId != null)
            {
                var CurrentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                if (CurrentUser != null)
                {
                    NewOrder.User = CurrentUser;
                    db.Orders.Add(NewOrder);
                    ViewBag.UserName = CurrentUser.UserName;
                    // отправка заказа рабочему
                    // находим рабочего у которого меньше всех заказов
                    ApplicationUser WorkerMin = new ApplicationUser();
                    WorkerMin = FindWorkerOrdersMin();
                    if (WorkerMin != null)
                        NewOrder.Worker = WorkerMin;
                    else View("Error! worker didn't receive the order");
                    db.SaveChanges();
                    return View("BookServiceComplete");
                }
            }       
            return View("Error");
        }

        public ApplicationUser FindWorkerOrdersMin()
        {
            ApplicationUser WorkerMin = new ApplicationUser();

            var Workers = db.Users.Where(role => role.RoleName == "worker");
            List<int> WorkerOrders = new List<int>();
            foreach (var item in Workers)
            {
                WorkerOrders.Add(item.Orders.Count());
            }
            int min = WorkerOrders.Min();
            foreach (var item in Workers)
            {
                if (item.Orders.Count() == min)
                    WorkerMin = item;
            }
            // если рабочего с минимальным количеством заказов получить не получилось, то выбираем случайного
            if(WorkerMin == null)
            {
                var random = new Random();
                int indexMin = random.Next(Workers.Count());
                WorkerMin = Workers.ElementAt(indexMin);
            }
                return WorkerMin;
        }
    }
}