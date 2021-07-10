using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Assignment_Team4.DAL;
using WEB_Assignment_Team4.Models;

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
                HttpContext.Session.SetInt32("criteriaCompNum", id.Value);
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
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["selectedCompetitionNo"] = HttpContext.Session.GetInt32("criteriaCompNum");
            return View();
        }

        // POST: CriteriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Criteria criteria)
        {
            ViewData["selectedCompetitionNo"] = HttpContext.Session.GetInt32("criteriaCompNum");
            if (ModelState.IsValid)
            {   
                //Add staff record to database
                criteria.CompetitionID = Convert.ToInt32(HttpContext.Session.GetInt32("criteriaCompNum"));
                if (criteriaContext.GetCriteriaTotal(criteria.CompetitionID) + criteria.Weightage <= 100)
                {
                    criteria.CriteriaID = criteriaContext.Add(criteria);
                    //Redirect user to Staff/Index view
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Total Weightage will be more than 100%!";
                    return View(criteria);
                }
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(criteria);
            }
        }

        // GET: CriteriaController/Edit/5
        public ActionResult Edit(int id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }

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
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }

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
