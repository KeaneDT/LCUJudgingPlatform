using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Assignment_Team4.DAL;
using WEB_Assignment_Team4.Models;

namespace WEB_Assignment_Team4.Controllers
{
    public class SelectionController : Controller
    {
        private InterestDAL interestContext = new InterestDAL();
        // GET: SelectionController
        public ActionResult Index(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            InterestViewModel interestVM = new InterestViewModel();
            interestVM.interestList = interestContext.GetAllInterest();
            if (id != null)
            {
                ViewData["selectedInterestNo"] = id.Value;
                interestVM.competitionList = interestContext.GetInterestCompetition(id.Value);
            }
            else
            {
                ViewData["selectedInterestNo"] = "";
            }
            return View(interestVM);
        }

        // GET: SelectionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SelectionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SelectionController/Create
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

        // GET: SelectionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SelectionController/Edit/5
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

        // GET: SelectionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SelectionController/Delete/5
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
