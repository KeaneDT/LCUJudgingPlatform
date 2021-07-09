using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Assignment_Team4.Models;
using WEB_Assignment_Team4.DAL;

namespace WEB_Assignment_Team4.Controllers
{
    public class CriteriaController : Controller
    {
        CompetitionDAL competitionContext = new CompetitionDAL();
        CriteriaDAL criteriaContext = new CriteriaDAL();

        // GET: CriteriaController
        public ActionResult Index(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }

            CompetitionCriteriaViewModel ccVM = new CompetitionCriteriaViewModel();
            ccVM.competitionList = competitionContext.GetJudgeCompetition(HttpContext.Session.GetString("LoginID"));

            ViewData["selectedCompetitionNo"] = "";
            if (id != null)
            {
                ViewData["selectedCompetitionNo"] = id.Value;
                // Get list of staff working in the branch
                ccVM.criteriaList = criteriaContext.GetCompetitionCriteria(id.Value);
            }
            else
            {
                ViewData["selectedCompetitionNo"] = "";
            }
            return View(ccVM);
        }

        // GET: CriteriaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CriteriaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CriteriaController/Create
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

        // GET: CriteriaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CriteriaController/Edit/5
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

        // GET: CriteriaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CriteriaController/Delete/5
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
