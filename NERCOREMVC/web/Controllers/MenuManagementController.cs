using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data;

namespace web.Controllers
{
    public class MenuManagementController : Controller
    {
        // GET: MenuManagement
        public ActionResult Index()
        {
            return View(new MenuDAO().GetAll());
        }

        // GET: MenuManagement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MenuManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MenuManagement/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MenuManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MenuManagement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MenuManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}