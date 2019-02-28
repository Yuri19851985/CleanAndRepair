using CleanAndRepair.Context;
using CleanAndRepair.Models;
using CleanAndRepair.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CleanAndRepair.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // действия админа с юзерами
        [Authorize(Roles = "admin")]
        public ActionResult UserList(string rolename="user")
        {
            List<UserListViewModel> models = new List<UserListViewModel>();
            var Users = db.Users.Where(role => role.RoleName == rolename);
            foreach (var item in Users)
            {
                UserListViewModel UnitModel = new UserListViewModel();
                UnitModel.user = item;
                // получаем заказы текущего рабочего
                var Orders = db.Orders.Where(order => order.User.Id == item.Id);
                if (Orders != null)
                {
                    UnitModel.CountOrders = Orders.Count();
                }
                models.Add(UnitModel);
            }
            return View(models);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteUser(string id)
        {
            var UserDel = db.Users.FirstOrDefault(i => i.Id == id);
            if (UserDel == null)
            {
                ViewBag.Message = "Error. Worker not found!";
                return View();
            }          
            db.Users.Remove(UserDel);
            db.SaveChanges();
            return RedirectToAction("UserList");
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditUser(string id)
        {
            var UserEdit = db.Users.FirstOrDefault(i => i.Id == id);
            if (UserEdit == null)
            {
                return View("Error. Worker not found!");
            }
            SelectList RoleList = new SelectList(db.Roles, "Name", "Name");
            if (RoleList == null)
            {
                return View("Error. Role not found!");
            }
            ViewBag.RoleList = RoleList;
            return View(UserEdit);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditUser(ApplicationUser model)
        {
            var UserEdit = db.Users.FirstOrDefault(i => i.Id == model.Id);
            if (UserEdit == null)
            {
                return View("Error. Worker not found!");
            }
            UserEdit.FullName = model.FullName;
            UserEdit.Email = model.Email;
            UserEdit.PhoneNumber = model.PhoneNumber;
            UserEdit.RoleName = model.RoleName;
            ChangeRoleUser(UserEdit.Id, UserEdit.RoleName); // замена роли пользователя
            UserEdit.Address = model.Address;
            UserEdit.Raiting = model.Raiting;
            db.SaveChanges();           

            return RedirectToAction("UserList");
        }

        // замена роли пользователя
        public bool ChangeRoleUser(string UserId, string RoleName)
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                            .GetUserManager<ApplicationUserManager>();
            var Roles = userManager.GetRoles(UserId);
            userManager.RemoveFromRoles(UserId, Roles.ToArray());
            var result = userManager.AddToRole(UserId, RoleName);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        // действия админа с работниками
        [Authorize(Roles = "admin")]
        public ActionResult WorkerList()
        {
            return RedirectToAction("UserList", new { rolename = "worker" });
        }
        [Authorize(Roles = "admin")]

        public ActionResult DeleteWorker(string id)
        {
            var UserDel = db.Users.FirstOrDefault(i => i.Id == id);
            if (UserDel == null)
            {
                return View("Error. Worker not found!");
            }
            db.Users.Remove(UserDel);
            db.SaveChanges();
            return RedirectToAction("WorkerList");
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditWorker(string id)
        {
            string currentUserId = User.Identity.GetUserId();
            var UserEdit = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (UserEdit == null)
            {
                return View("Error. Worker not found!");
            }
            SelectList RoleList = new SelectList(db.Roles, "Name", "Name");
            if (RoleList == null)
            {
                return View("Error. Role not found!");
            }
            ViewBag.RoleList = RoleList;
            return View(UserEdit);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditWorker(ApplicationUser model)
        {
            var UserEdit = db.Users.FirstOrDefault(i => i.Id == model.Id);
            if (UserEdit == null)
            {
                return View("Error. Worker not found!");
            }
            UserEdit.FullName = model.FullName;
            UserEdit.Email = model.Email;
            UserEdit.PhoneNumber = model.PhoneNumber;
            UserEdit.RoleName = model.RoleName;
            ChangeRoleUser(UserEdit.Id, UserEdit.RoleName); // замена роли работника
            UserEdit.Address = model.Address;
            UserEdit.Raiting = model.Raiting;
            db.SaveChanges();
            return RedirectToAction("WorkerList");
        }

        // действия админа с заказами
        [Authorize(Roles = "admin")]
        public ActionResult UserOrders(string id)
        {
            var Orders = db.Orders.Where(order => order.User.Id == id);
            if (Orders.Count() != 0)
            {
                return View(Orders);
            }
            return View();            
        }

        public ActionResult WorkerOrders(string id)
        {
            var Orders = db.Orders.Where(order => order.Worker.Id == id);
            if (Orders.Count() != 0)
            {
                return View(Orders);
            }
            return View();
        }

        public ActionResult DeleteOrder(int id, string UserId)
        {
            Order OrderDelete = db.Orders.Find(id);
            if (OrderDelete != null)
            {
                db.Orders.Remove(OrderDelete);
                db.SaveChanges();
            }
            else return View("Error. Такого заказа не существует!");
            return RedirectToAction("UserList");
        }

        //действия админа с услугами
        [Authorize(Roles = "admin")]
        public ActionResult ServiceList()
        {
            var ServiceList = db.Services;
            if (ServiceList == null)
            {
                return View("Error. Service not found!");
            }
            return View(ServiceList);
        }

        [Authorize(Roles = "admin")]
        public ActionResult CreateService()
        {
            SelectList GroupList = new SelectList(db.Groups, "Id", "Name");
            if (GroupList == null)
            {
                return View("Error. Group not found!");
            }           
            ViewBag.GroupList = GroupList;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult CreateService(Service model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (db.Services.FirstOrDefault(m => m.Name == model.Name) != null)
            {
                ViewBag.Message = "Такая услуга уже существует в базе";
                return View(model);
            }

            var Service = new Service();
            Service = model;
            //поиск выбранной группы по айдишнику переданному из селектлиста
            var Group = db.Groups.FirstOrDefault(group => group.Id == model.Group.Id);
            if (Group == null)
            {
                return View(model);
            }
            else Service.Group = Group;
            db.Services.Add(Service);
            db.SaveChanges();
            return RedirectToAction("ServiceList");
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteService(int id)
        {
            var ServiceDel = db.Services.FirstOrDefault(i => i.Id == id);
            if (ServiceDel == null)
            {
                return View("Error. Group not found!");
            }
            db.Services.Remove(ServiceDel);
            db.SaveChanges();
            return RedirectToAction("ServiceList");
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditService(int id)
        {
            var ServiceEdit = db.Services.FirstOrDefault(i => i.Id == id);
            if (ServiceEdit == null)
            {
                return View("Error. Group not found!");
            }
            SelectList GroupList = new SelectList(db.Groups, "Id", "Name");
            if (GroupList == null)
            {
                return View("Error. Group not found!");
            }
            ViewBag.GroupList = GroupList;
            return View(ServiceEdit);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditService(Service model)
        {
            var ServiceEdit = db.Services.FirstOrDefault(i => i.Id == model.Id);
            if (ServiceEdit == null)
            {
                return View("Error. Service not found!");
            }
            ServiceEdit.Name = model.Name;
            ServiceEdit.Price = model.Price;
            var Group = db.Groups.FirstOrDefault(group => group.Id == model.Group.Id);
            if (Group == null)
            {
                return View(model);
            }
            else ServiceEdit.Group = Group;
            db.SaveChanges();
            return RedirectToAction("ServiceList");
        }

        //действия админа с группами
        [Authorize(Roles = "admin")]
        public ActionResult GroupList()
        {
            var GroupList = db.Groups;
            if (GroupList == null)
            {
                return View("Error. Group not found!");
            }
            return View(GroupList);
        }

        [Authorize(Roles = "admin")]
        public ActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult CreateGroup(GroupService model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (db.Groups.FirstOrDefault(m => m.Name == model.Name) != null)
            {
                ViewBag.Message = "Такая группа уже существует в базе";
                return View(model);
            }
            var Group = new GroupService();
            Group = model;           
            db.Groups.Add(Group);
            db.SaveChanges();
            return RedirectToAction("GroupList");
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteGroup(int id)
        {
            var GroupDel = db.Groups.FirstOrDefault(i => i.Id == id);
            if (GroupDel == null)
            {
                return View("Error. Group not found!");
            }
            db.Groups.Remove(GroupDel);
            db.SaveChanges();
            return RedirectToAction("GroupList");
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditGroup(int id)
        {
            var GroupEdit = db.Groups.FirstOrDefault(i => i.Id == id);
            if (GroupEdit == null)
            {
                return View("Error. Group not found!");
            }
            return View(GroupEdit);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditGroup(GroupService model)
        {
            var GroupEdit = db.Groups.FirstOrDefault(i => i.Id == model.Id);
            if (GroupEdit == null)
            {
                return View("Error. Group not found!");
            }
            GroupEdit.Name = model.Name;           
            db.SaveChanges();
            return RedirectToAction("GroupList");
        }

    }
}