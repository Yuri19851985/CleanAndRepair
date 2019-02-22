using CleanAndRepair.Context;
using CleanAndRepair.Models;
using CleanAndRepair.ViewModels;
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

        [Authorize(Roles = "admin")]
        public ActionResult WorkerList()
        {
            List<WorkerListViewModel> models = new List<WorkerListViewModel>();
            var Workers = db.Workers;
            foreach(var item in Workers)
            {
                WorkerListViewModel UnitModel = new WorkerListViewModel();
                UnitModel.worker = item;                
                // получаем заказы текущего рабочего
                var Orders = db.Orders.Where(order => order.Worker.Id == item.Id);
                if(Orders != null)
                {
                    UnitModel.CountOrders = Orders.Count();
                }
                models.Add(UnitModel);
            }
            return View(models);
        }

        [Authorize(Roles = "admin")]
        public ActionResult CreateWorker()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult CreateWorker(Worker model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (db.Workers.FirstOrDefault(m => m.Phone == model.Phone) != null)
            {
                ViewBag.Message = "Пользователь с таким телефоном уже существует в базе";
                return View(model);
            }
            var NewWorker = new Worker();
            NewWorker = model;
            NewWorker.Orders = new List<Order>();
            db.Workers.Add(NewWorker);
            db.SaveChanges();
            return RedirectToAction("WorkerList");
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteWorker(int id)
        {
            var WorkerDel = db.Workers.FirstOrDefault(i => i.Id == id);
            if (WorkerDel == null)
            {
                return View("Error. Worker not found!");
            }
            db.Workers.Remove(WorkerDel);
            db.SaveChanges();
            return RedirectToAction("WorkerList");
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditWorker(int id)
        {
            var WorkerEdit = db.Workers.FirstOrDefault(i => i.Id == id);
            if (WorkerEdit == null)
            {
                return View("Error. Worker not found!");
            }
            return View(WorkerEdit);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditWorker(Worker model)
        {
            var WorkerEdit = db.Workers.FirstOrDefault(i => i.Id == model.Id);
            if (WorkerEdit == null)
            {
                return View("Error. Worker not found!");
            }
            WorkerEdit.Name = model.Name;
            WorkerEdit.Phone = model.Phone;
            WorkerEdit.Raiting = model.Raiting;
            db.SaveChanges();
            return RedirectToAction("WorkerList");
        }

        [Authorize(Roles = "admin")]
        public ActionResult DetailsWorker(int id)
        {
            WorkerDetailsViewModel model = new WorkerDetailsViewModel();
            model.worker = db.Workers.FirstOrDefault(i => i.Id == id);
            var Orders = db.Orders.Where(order => order.Worker.Id == id);
            if (Orders != null)
            {
                model.Orders.AddRange(Orders);
            }
            else { return View(ViewBag.Message = "Список заказов пуст"); }

            return View(model);
        }

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
            if (db.Workers.FirstOrDefault(m => m.Name == model.Name) != null)
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