using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_Assignment_Team4.DAL;
using WEB_Assignment_Team4.Models;

namespace WEB_Assignment_Team4.Controllers
{
    public class AssignController : Controller
    {
        private JudgeDAL judgeContext = new JudgeDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();

        // GET: AssignController
        public ActionResult Index()
        {
            // Stop Accessing the action if not logged in
            //or account not in the "Administrator" role
            if (HttpContext.Session.GetString("Role") == null ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<JudgeAssign> judgeList = judgeContext.GetAllAssignJudges();
            return View(judgeList);
        }

        // GET: AssignController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AssignController/Create
        public ActionResult Assign(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Administrator" Role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["judgeList"] = GetJudges();
            ViewData["competitionList"] = GetCompetition();
            return View();
        }
        private List<Judge> GetJudges()
        {
            List<Judge> judgeList = judgeContext.GetAllJudges();
            judgeList.Insert(0, new Judge
            {
                JudgeID = 0,
                JudgeName = "Select Name"
            });
            return judgeList;
        }
        private List<Competition> GetCompetition()
        {
            List<Competition> competitionList = competitionContext.GetAllCompetition();
            competitionList.Insert(0, new Competition
            {
                CompetitionID = 0,
                Name = "Select Competition"
            });
            return competitionList;
        }
        // POST: AssignController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(JudgeAssign judgeAssign)
        {
            ViewData["judgeList"] = GetJudges();
            ViewData["competitionList"] = GetCompetition();

            if (ModelState.IsValid)
            {
                judgeAssign.CompetitionID = judgeContext.Assign(judgeAssign);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Select One Interest from the list. ";
                return RedirectToAction("Assign");
            }
        }

        // GET: AssignController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AssignController/Edit/5
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

        // GET: AssignController/Delete/5
        public ActionResult AssignDelete(int? id)
        {
            //Stop accessing the action if not logged in
            //or account not in the "Administrator" role
            if ((HttpContext.Session.GetString("Role") == null) ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Competition");
            }
            if (id == null)
            {
                //Return to the index page, not allowed to edit
                return RedirectToAction("Index");
            }
            JudgeAssign role = judgeContext.GetJudgesRole(id.Value);
            if (role == null)
            {
                //Return to the index page, not allowed to edit
                return RedirectToAction("Index");
            }
            judgeContext.AssignDelete(role.CompetitionID);
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignDelete(JudgeAssign judgeAssign)
        {
            //Delete the records from the database
            judgeContext.AssignDelete(judgeAssign.CompetitionID);
            TempData["Message"] = "Records Added Successfully. ";
            return RedirectToAction("Index");
        }
    }
}