using Autofac;
using ppedv.QualitySlegehammer.Logic;
using ppedv.QualitySlegehammer.Model;
using ppedv.QualitySlegehammer.Model.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ppedv.QualitySlegehammer.UI.Web.Controllers
{
    public class OrderController : Controller
    {
        Core core;



        public OrderController()
        {
            var builder = new ContainerBuilder();

            var dataAsm = Assembly.LoadFrom(@"C:\Users\ar2\source\repos\ppedvAG\01042019_Arch_NBG\ppedv.QualitySlegehammer\ppedv.QualitySlegehammer.Data.EF\bin\Debug\ppedv.QualitySlegehammer.Data.EF.dll");
            builder.RegisterAssemblyTypes(dataAsm).As<IUnitOfWork>().InstancePerLifetimeScope();

            var deviceAsm = Assembly.LoadFrom(@"C:\Users\ar2\source\repos\ppedvAG\01042019_Arch_NBG\ppedv.QualitySlegehammer\ppedv.QualitySlegehammer.Data.BinFord\bin\Debug\netstandard2.0\BinFord.MegaBeeper5000.dll");
            builder.RegisterAssemblyTypes(deviceAsm).As<IDevice>().InstancePerLifetimeScope();

            //builder.RegisterType<Core>().UsingConstructor(typeof(IUnitOfWork), );

            var container = builder.Build();

            core = new Core(container.Resolve<IUnitOfWork>());


        }

        //public OrderController()
        //{
        //    core = new Core();
        //}

        // GET: Order
        public ActionResult Index()
        {
            return View(core.UnitOfWork.OrderRepository.Query().ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            return View(core.UnitOfWork.OrderRepository.GetById(id));
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View(new Order() { ProdNr = "NEU" });
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(Order order)
        {
            try
            {
                core.UnitOfWork.OrderRepository.Add(order);
                core.UnitOfWork.SaveAll();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View(core.UnitOfWork.OrderRepository.GetById(id));
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Order order)
        {
            try
            {
                core.UnitOfWork.OrderRepository.Update(order);
                core.UnitOfWork.SaveAll();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View(core.UnitOfWork.OrderRepository.GetById(id));
        }

        // POST: Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Order order)
        {
            try
            {
                var loaded = core.UnitOfWork.OrderRepository.GetById(id);
                if (loaded != null)
                {
                    core.UnitOfWork.OrderRepository.Delete(loaded);
                    core.UnitOfWork.SaveAll();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
