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
    public class ScoreController : Controller
    {
        //Declare DAL Objects to use SQL Commands in the Actions
        CompetitionDAL competitionContext = new CompetitionDAL();
        SubmissionsDAL submissionsContext = new SubmissionsDAL();

        public ActionResult Index(int? id)
        {
            //Check if the Role of the user is a Judge
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }

            //Declare CompetitionSubmissionViewModel
            CompetitionSubmissionViewModel csVM = new CompetitionSubmissionViewModel();
            //Add the list of competitions the Judge is in through the use of SELECT commands
            csVM.competitionList = competitionContext.GetJudgeCompetition(HttpContext.Session.GetString("LoginID"));

            //Set the SelectedCompetitionNo to "" which means nothing is selected
            ViewData["selectedCompetitionNo"] = "";
            //Check the selected CompetitionID that has been passed to the query string
            if (id != null)
            {
                ViewData["selectedCompetitionNo"] = id.Value;
                ViewData["competitionName"] = competitionContext.GetDetails(id.Value).Name;
                //Set the Competition ID Selected to be used in other actions
                HttpContext.Session.SetInt32("criteriaSubNum", id.Value);
                // Get list of Criteria for the Competition
                csVM.submissionsList = submissionsContext.GetCompetitionSubmissions(id.Value);
            }
            else
            {
                ViewData["selectedCompetitionNo"] = "";
            }
            return View(csVM);
        }

        // GET: ScoreController/Details/5
        public ActionResult Details(int competitionID, int competitorID, string fileName)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            SubmissionViewModel sVM = submissionsContext.GetSubmissionDetails(competitionID, competitorID, fileName);
            return View(sVM);
        }

        // GET: ScoreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ScoreController/Edit/5
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
    }
}
