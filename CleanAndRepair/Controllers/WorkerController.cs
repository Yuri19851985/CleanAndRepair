using CleanAndRepair.Context;
using CleanAndRepair.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CleanAndRepair.Controllers
{
    public class WorkerController : Controller
    {
        // GET: Worker
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "worker")]
        public ActionResult WorkerProfile()
        {
            string currentUserId = User.Identity.GetUserId();
            var UserEdit = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View(UserEdit);
        }

        [Authorize(Roles = "worker")]
        public ActionResult EditProfile()
        {
            string currentUserId = User.Identity.GetUserId();
            var UserEdit = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (UserEdit == null)
            {
                return View("Error. User not found!");
            }
            return View(UserEdit);
        }

        [Authorize(Roles = "worker")]
        [HttpPost]
        public ActionResult EditProfile(ApplicationUser model)
        {
            string currentUserId = User.Identity.GetUserId();
            var UserEdit = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (UserEdit == null)
            {
                return View("Error. Worker not found!");
            }
            UserEdit.FullName = model.FullName;
            UserEdit.Email = model.Email;
            UserEdit.PhoneNumber = model.PhoneNumber;
            UserEdit.Address = model.Address;
            UserEdit.Raiting = model.Raiting;
            db.SaveChanges();
            var Userid = UserEdit.Id;
            return RedirectToAction("UserProfile");
        }

        [Authorize(Roles = "worker")]
        public ActionResult OrderListIdentityWorker()
        {
            string currentUserId = User.Identity.GetUserId();
            var CurrentIdentityUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            // выбираем список заказов с Id текущего пользователя
            var OrdersCurrentUser = db.Orders.Where(order => order.Worker.Id == CurrentIdentityUser.Id);
            foreach (var item in OrdersCurrentUser)
            {
                var OrderService = db.Services.FirstOrDefault(service => service.Id == item.ServiceOrder.Id);
                if (OrderService != null)
                {
                    item.ServiceOrder = OrderService;
                }
            }
            return View(OrdersCurrentUser);
        }
        public ActionResult GetRaiting()
        {
            ViewBag.RaitingValue = GetRaitingValueCurrentUser().ToString();
            return View();
        }

        public double GetRaitingValueCurrentUser()
        {
            string currentUserId = User.Identity.GetUserId();
            var CurrentIdentityUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            double raiting = CurrentIdentityUser.Raiting;
            return raiting;
        }

        public ActionResult OrderComplete(int id)
        {
            var Order = db.Orders.FirstOrDefault(or => or.Id == id);
            if(Order != null && Order.Complete == false)
            {
                Order.Complete = true;
                db.SaveChanges();
            }
            return RedirectToAction("OrderListIdentityWorker");
        }
        public ActionResult OrderDetails(int id)
        {
            var Order = db.Orders.FirstOrDefault(or => or.Id == id);
            if (Order != null)
            {
                ViewBag.RaitingValue = GetRaitingValueCurrentUser().ToString();
                return View(Order);
            }
            return View();
        }

        // отправка уведомления на email о выезде специалиста
        public ActionResult OrderEmailNotification(int id)
        {
            var Order = db.Orders.FirstOrDefault(o => o.Id == id);
            var User = db.Users.FirstOrDefault(u => u.Id == Order.User.Id);
            var Worker = db.Users.FirstOrDefault(w => w.Id == Order.Worker.Id);

            EmailMessage message = new EmailMessage();
            message.From = "cleanandrepairtest@yandex.ru";
            message.To = User.Email;
            message.Subject = "Выезд специалиста";
            message.Body = "Здравствуйте! К Вам выехал специалист для оказания услуги. Пожалуйста оставайтесь дома до его приезда!";
            try
            {
                new EmailController().SendEmail(message).Deliver();
                return RedirectToAction("Success");
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}