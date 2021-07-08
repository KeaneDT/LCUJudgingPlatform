using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Assignment_Team4.Controllers
{
    public class SubmissionsController : Controller
    {
        // GET: SubmissionsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SubmissionsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubmissionsController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Select()
        {
            return View();
        }


        // POST: SubmissionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SubmissionsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubmissionsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SubmissionsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubmissionsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
