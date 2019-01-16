using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CleanAndRepair.Models;
using CleanAndRepair.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CleanAndRepair.Context
{
    public class Initializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            GroupService group1 = new GroupService() { Name = "Уборка", Services = new List<Service>() };
            group1.Services.Add(new Service() { Name = "Уборка помещения", Price = 30, Count = 1 });
            group1.Services.Add(new Service() { Name = "Чистка кухни", Price = 25, Count = 1 });
            group1.Services.Add(new Service() { Name = "Удаление плесени", Price = 27, Count = 1, });
            group1.Services.Add(new Service() { Name = "Борьба с вредителями", Price = 33, Count = 1 });
            group1.Services.Add(new Service() { Name = "Чистка сантехники", Price = 28, Count = 1 });
            context.Groups.Add(group1);

            GroupService group2 = new GroupService() { Name = "ТВ и Электроника", Services = new List<Service>() };
            group2.Services.Add(new Service() { Name = "Монтаж ТВ на стену", Price = 15, Count = 1 });
            group2.Services.Add(new Service() { Name = "Настройка ТВ", Price = 40, Count = 1 });
            group2.Services.Add(new Service() { Name = "Установка камеры видеонаблюдения", Price = 100, Count = 1 });
            context.Groups.Add(group2);

            GroupService group3 = new GroupService() { Name = "Сборка", Services = new List<Service>() };
            group3.Services.Add(new Service() { Name = "Сборка мебели", Price = 50, Count = 1 });
            group3.Services.Add(new Service() { Name = "Сборка тренажеров", Price = 80, Count = 1 });
            context.Groups.Add(group3);

            GroupService group4 = new GroupService() { Name = "Сантехника", Services = new List<Service>() };
            group4.Services.Add(new Service() { Name = "Установка крана или смесителя", Price = 35, Count = 1 });
            group4.Services.Add(new Service() { Name = "Установка ванны", Price = 60, Count = 1 });
            group4.Services.Add(new Service() { Name = "Монтаж душевой кабины", Price = 220, Count = 1 });
            group4.Services.Add(new Service() { Name = "Ремонт крана", Price = 45, Count = 1 });
            group4.Services.Add(new Service() { Name = "Ремонт слива", Price = 48, Count = 1 });
            group4.Services.Add(new Service() { Name = "Ремонт унитаза", Price = 55, Count = 1 });
            group4.Services.Add(new Service() { Name = "Ремонт ванны", Price = 62, Count = 1 });
            context.Groups.Add(group4);

            GroupService group5 = new GroupService() { Name = "Электрика", Services = new List<Service>() };
            group5.Services.Add(new Service() { Name = "Установка световых точек", Price = 45, Count = 1 });
            group5.Services.Add(new Service() { Name = "Установка выключателя", Price = 11, Count = 1 });
            group5.Services.Add(new Service() { Name = "Установка розетки", Price = 11, Count = 1 });
            context.Groups.Add(group5);

            GroupService group6 = new GroupService() { Name = "Маляры", Services = new List<Service>() };
            group6.Services.Add(new Service() { Name = "Покраска стен", Price = 45, Count = 1 });
            group6.Services.Add(new Service() { Name = "Покраска потолков", Price = 65, Count = 1 });
            group6.Services.Add(new Service() { Name = "Покраска плинтусов", Price = 25, Count = 1 });
            group6.Services.Add(new Service() { Name = "Настенные рисунки", Price = 310, Count = 1 });
            context.Groups.Add(group6);

            GroupService group7 = new GroupService() { Name = "IT-сервис", Services = new List<Service>() };
            group7.Services.Add(new Service() { Name = "Переустановка ОС", Price = 50, Count = 1 });
            group7.Services.Add(new Service() { Name = "Чистка ноутбука", Price = 60, Count = 1 });
            group7.Services.Add(new Service() { Name = "Ремонт ноутбука", Price = 100, Count = 1 });
            context.Groups.Add(group7);

            GroupService group8 = new GroupService() { Name = "Грузчики", Services = new List<Service>() };
            group8.Services.Add(new Service() { Name = "Погрузка/разгрузка", Price = 110, Count = 1 });
            group8.Services.Add(new Service() { Name = "Подъем грузов на этаж", Price = 25, Count = 1 });
            context.Groups.Add(group8);

            GroupService group9 = new GroupService() { Name = "Окна", Services = new List<Service>() };
            group9.Services.Add(new Service() { Name = "Регулировка стеклопакетов", Price = 30, Count = 1 });
            group9.Services.Add(new Service() { Name = "Обследование стеклопакетов", Price = 15, Count = 1 });
            group9.Services.Add(new Service() { Name = "Ремонт стеклопакетов", Price = 70, Count = 1 });
            group9.Services.Add(new Service() { Name = "Монтаж карниза", Price = 60, Count = 1 });
            group9.Services.Add(new Service() { Name = "Установка ролштор", Price = 25, Count = 1 });
            context.Groups.Add(group9);

            GroupService group10 = new GroupService() { Name = "Садовники", Services = new List<Service>() };
            group10.Services.Add(new Service() { Name = "Стрижка газона", Price = 80, Count = 1 });
            group10.Services.Add(new Service() { Name = "Уборка двора", Price = 90, Count = 1 });
            context.Groups.Add(group10);

            //var user1 = new ApplicationUser { UserName = "Yuri", Email = "yuri-zhurakovski@yandex.ru" };

            //var role1 = new ApplicationRole { Name = "admin" };
            //var role2 = new ApplicationRole { Name = "user" };
            //context.Roles.Add(role1);
            //context.Roles.Add(role2);
            //user1.Roles.Add(role1);

            context.SaveChanges();

            //var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //// создаем две роли
            //var role1 = new IdentityRole { Name = "admin" };
            //var role2 = new IdentityRole { Name = "user" };

            //// добавляем роли в бд
            //roleManager.Create(role1);
            //roleManager.Create(role2);

            //// создаем пользователей
            //var admin = new ApplicationUser
            //{
            //    Email = "yuri-zhurakovski@yandex.ru",
            //    UserName = "yuri-zhurakovski@yandex.ru"
            //};
            //string password = "000000";
            //var result = userManager.Create(admin, password);

            ////если создание пользователя прошло успешно
            //if (result.Succeeded)
            //{
            //    //добавляем для пользователя роль
            //    userManager.AddToRole(admin.Id, role1.Name);
            //    userManager.AddToRole(admin.Id, role2.Name);
            //}

            //base.Seed(context);

        }
    }
}