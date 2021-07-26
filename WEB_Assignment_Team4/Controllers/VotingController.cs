using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Assignment_Team4.Controllers
{
    public class VotingController : Controller
    {
        // GET: VotingController
        public ActionResult Index()
        {
            return View();
        }

        // GET: VotingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VotingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VotingController/Create
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

        // GET: VotingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VotingController/Edit/5
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

        // GET: VotingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VotingController/Delete/5
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
