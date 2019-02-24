using CleanAndRepair.Context;
using CleanAndRepair.Models;
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
            string NameCurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
            var CurrentIdentityUser = db.Users.FirstOrDefault(User => User.UserName.Equals(NameCurrentUser));
            return View(CurrentIdentityUser);
        }

        [Authorize(Roles = "user")]
        public ActionResult EditProfile()
        {
            string NameCurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
            var UserEdit = db.Users.FirstOrDefault(i => i.UserName == NameCurrentUser);
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
            var UserEdit = db.Users.FirstOrDefault(i => i.Id == model.Id);
            if (UserEdit == null)
            {
                return View("Error. Worker not found!");
            }
            UserEdit.UserName = model.UserName;
            UserEdit.Email = model.Email;
            UserEdit.PhoneNumber = model.PhoneNumber;
            UserEdit.RoleName = model.RoleName;
            UserEdit.Address = model.Address;
            UserEdit.Raiting = model.Raiting;
            db.SaveChanges();
            return RedirectToAction("UserProfile");
        }

        [Authorize(Roles = "user")]
        public ActionResult OrderListIdentityUser()
        {
            string NameCurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
            var CurrentIdentityUser = db.Users.FirstOrDefault(User => User.UserName.Equals(NameCurrentUser));
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


    }
}