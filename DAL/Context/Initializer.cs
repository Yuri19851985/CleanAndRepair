using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DAL.Context
{
    public class Initializer : DropCreateDatabaseAlways<Context>
    {
        protected override void Seed(Context context)
        {        
            context.Groups.Add(new GroupService() { Name = "Уборка", Services = new List<Service>() });
            context.Groups.Add(new GroupService() { Name = "ТВ и Электроника", Services = new List<Service>() });
            context.Groups.Add(new GroupService() { Name = "Сборка", Services = new List<Service>() });
            context.Groups.Add(new GroupService() { Name = "Сантехника", Services = new List<Service>() });
            context.Groups.Add(new GroupService() { Name = "Электрика", Services = new List<Service>() });
            context.Groups.Add(new GroupService() { Name = "Маляры", Services = new List<Service>() });
            context.Groups.Add(new GroupService() { Name = "IT-сервис", Services = new List<Service>() });
            context.Groups.Add(new GroupService() { Name = "Грузчики", Services = new List<Service>() });
            context.Groups.Add(new GroupService() { Name = "Окна", Services = new List<Service>() });
            context.Groups.Add(new GroupService() { Name = "Садовники", Services = new List<Service>() });


            Service service1 = new Service() { Name = "Уборка помещения", Price = 30, Count = 1,
                Professionals = new List<Professional>() };
            Service service2 = new Service()
            {
                Name = "Чистка сантехники",
                Price = 20,
                Count = 1,
                Professionals = new List<Professional>()
            };
        }
    }
}