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
        private InterestDAL interestContext = new InterestDAL();
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

        public ActionResult AllJudges()
        {
            // Stop Accessing the action if not logged in
            //or account not in the "Administrator" role
            if (HttpContext.Session.GetString("Role") == null ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Judge> judgeList = judgeContext.GetAllJudges();
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

            try
            {
                judgeAssign.CompetitionID = judgeContext.Assign(judgeAssign);
                TempData["Message"] = "Judge Competition Has been assigned. ";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //created an error message and store to tempdata 
                TempData["Message"] = "The Form is empty. Fill in the required blank before" +
                                      " assigning the judges to their respective competition.";
                //return to index page to view the interest list and display an message
                return View(judgeAssign);

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
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Return to the index page, not allowed to edit
                return RedirectToAction("Index");
            }
            JudgeAssign judgeAssign = judgeContext.GetJudgesRole(id.Value);
            if (judgeAssign == null)
            {
                //Return to the index page, not allowed to edit
                return RedirectToAction("Index");
            }

            return View(judgeAssign);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignDelete(JudgeAssign judgeAssign,int? id)
        {
            judgeAssign = judgeContext.GetJudgesRole(id.Value);
            //Delete the records from the database
            judgeContext.AssignDelete(judgeAssign.CompetitionID, judgeAssign.JudgeID);
            TempData["Message"] = "Judge Records Deleted Successfully. ";
            return RedirectToAction("Index");
        }
        public ActionResult JudgeDetails(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            Judge judge = judgeContext.GetDetails(id);
            JudgeViewModel judgeVM = MapToJudgeVM(judge);
            return View(judgeVM);
        }

        public JudgeViewModel MapToJudgeVM(Judge judge)
        {
            string areaInterestName = "";
            if (judge.AreaInterestID != null)
            {
                List<Interest> interestList = interestContext.GetAllInterest();
                foreach (Interest interest in interestList)
                {
                    if (interest.AreaInterestID == judge.AreaInterestID.Value)
                    {
                        areaInterestName = interest.Name;
                        break;
                    }
                }
            }

            JudgeViewModel competitionVM = new JudgeViewModel
            {
                JudgeID= judge.JudgeID,
                JudgeName = judge.JudgeName,
                Salutation = judge.Salutation,
                AreaInterestName = areaInterestName,
                EmailAddr = judge.EmailAddr,
                Password = judge.Password
            };
            return competitionVM;
        }
    }
}