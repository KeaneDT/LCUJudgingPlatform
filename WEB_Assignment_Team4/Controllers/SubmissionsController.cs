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
    public class SubmissionsController : Controller
    {
        private SubmissionsDAL submissionContext = new SubmissionsDAL();

        // GET: SubmissionsController

        public ActionResult Index(int? id)
        {
            Submissions submissions = new Submissions();
            submissions.submissionsList = submissionContext.GetAllSubmissions();
            // Check if BranchNo (id) presents in the query string
            string voteStatus = HttpContext.Session.GetString("voteStatus");
            if (voteStatus == null)
            {
                if (id != null)
                {
                    ViewData["selectedCompetition"] = id.Value;
                    // Get list of staff working in the branch
                    submissions.submissionsList = submissionContext.GetCompetitionSubmissions(id.Value);
                }
                else
                {
                    ViewData["selectedCompetition"] = "";
                }
                return View(submissions);
            }
            else
            {
                ViewData["selectedCompetition"] = id.Value;
                TempData["Message"] = "Thank you for voting! Vote again next time";
                return View(submissions);
            }
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

        public ActionResult Edit(int competitionID, int competitorID)
        {
            string voteStatus = HttpContext.Session.GetString("voteStatus");
            if(voteStatus == null)
            {
                ViewData["SubmissionList"] = submissionContext.GetAllSubmissions();
                if (ModelState.IsValid)
                {
                    //Update staff record to database
                    submissionContext.IncreaseCount(competitionID, competitorID);
                    HttpContext.Session.SetString("voteStatus", "Voted");
                    TempData["Message"] = "Thank you for voting! Vote again next time";
                    return RedirectToAction("Index", new { id = competitionID });
                }
                else
                {
                    //Input validation fails, return to the view
                    //to display error message
                    return RedirectToAction("Index", new { id = competitionID });
                }
            }
            else
            {
                return RedirectToAction("Index", new { id = competitionID });
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
