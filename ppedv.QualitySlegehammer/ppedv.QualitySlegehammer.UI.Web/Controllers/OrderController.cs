using ppedv.QualitySlegehammer.Logic;
using ppedv.QualitySlegehammer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ppedv.QualitySlegehammer.UI.Web.Controllers
{
    public class OrderController : Controller
    {
        Core core = new Core();

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
