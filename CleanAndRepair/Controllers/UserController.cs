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
    public class UserController : Controller
    {
        // GET: User
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "user")]
        public ActionResult UserProfile()
        {
            string currentUserId = User.Identity.GetUserId();
            var UserEdit = db.Users.FirstOrDefault(x => x.Id == currentUserId);                     
            return View(UserEdit);
        }

        [Authorize(Roles = "user")]
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

        [Authorize(Roles = "user")]
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
            return RedirectToAction("UserProfile");
        }

        [Authorize(Roles = "user")]
        public ActionResult OrderListIdentityUser()
        {
            string currentUserId = User.Identity.GetUserId();
            var CurrentIdentityUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            // выбираем список заказов с Id текущего пользователя
            var OrdersCurrentUser = db.Orders.Where(order => order.User.Id == CurrentIdentityUser.Id);
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

        [Authorize(Roles = "user")]
        public ActionResult DeleteOrder(int id)
        {
            Order OrderDelete = db.Orders.Find(id);
            if (OrderDelete != null)
            {
                db.Orders.Remove(OrderDelete);
                db.SaveChanges();
            }
            else return View("Error. Такого заказа не существует!");
            return RedirectToAction("OrderListIdentityUser");
        }

        [Authorize(Roles = "user")]
        public ActionResult SetRaitingForWorker(int id)
        {
            return View();
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public ActionResult SetRaitingForWorker(Order order)
        {
            if(order.UserSetRaiting == 0)
            {
                return RedirectToAction("OrderListIdentityUser");
            }
            var Order = db.Orders.FirstOrDefault(or => or.Id == order.Id);
            if (Order == null)
                return RedirectToAction("OrderListIdentityUser");
            Order.UserSetRaiting = order.UserSetRaiting;
            var Worker = db.Users.FirstOrDefault(user => user.Id == Order.Worker.Id);
            if(Worker == null)
                return RedirectToAction("OrderListIdentityUser");
            // вычисление рейтинга рабочего
            var WorkerOrders = db.Orders.Where(ord => ord.Worker.Id == Worker.Id);
            if (WorkerOrders == null)
                return RedirectToAction("OrderListIdentityUser");
            Worker.Raiting = WorkerOrders.Sum(ord => ord.UserSetRaiting)/WorkerOrders.Count();
            db.SaveChanges();
            return View();
        }


    }
}